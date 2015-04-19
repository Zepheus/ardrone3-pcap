# ArDrone3 Packet Sniffer #
![Screenshot](/assets/screenshot.png?raw=true "Screenshot")
ArDrone3 packet sniffer is a protocol sniffer based on the ArDrone3 protocol as specified by Parrot.
It allows flexible analysis for the development of 3rd party ArDrone software.

This library depends on winpcap/libpcap (Linux) which can be obtained here: [WinPcap](https://www.winpcap.org/) or through your favorite package manager. Bindings are provided by SharpPcap. (Mono build guidelines: [SharpPcap Readme](https://github.com/rubystream/SharpPcap/blob/master/SharpPcap/Readme.Mono))

For Windows, Visual Studio and NuGet is required. Make sure you build as ``x86`` and not for ``All CPU``.


# ARDRONE 3.0 Protocol Specification #

**Warning**: This is a draft, carefully constructed from the source files from Parrot
(reference: [BebopDroneReceiveStream.c](https://github.com/ARDroneSDK3/Samples/blob/master/Unix/BebopDroneReceiveStream/BebopDroneReceiveStream.c))

## Frame structure ##
- 1 byte type
- 1 byte id
- 1 byte seq
- 1 unsigned integer frame size, 7 header bytes included
- Data (size)

## Frame processing ##
Each frame has an id. Packet ID < 2 is reserved for internal traffic:

- ID = 0, Used for internal PING
- ID = 1, Used for internal PONG

For more information about ping/pong, see [Ping & Pong](#ping--pong)

A frame type is used to determine which type of data is sent.
We can distinguish following types:

- Type 1, ARNETWORKAL\_FRAME\_TYPE\_ACK: An ack packet should be replied using the sequence num data. This is the first byte of the frame's data content. See [ACK Frames](#ack-frames)

- Type 2, ARNETWORKAL\_FRAME\_TYPE\_DATA:
Depending on the frame's sequence number, the packet is accepted and processed. It is then passed to the packet processor. See [Packet Processing](#packet-processing).

- Type 3, ARNETWORKAL\_FRAME\_TYPE\_DATA\_LOW_LATENCY:
Video data, see [Video Packets](#video-packets)

- Type 4, ARNETWORKAL\_FRAME\_TYPE\_DATA\_WITH\_ACK:
Process as above, but send ACK even when seq is not accepted! See [ACK Frames](#ack-frames)

Seq is seperately managed for each ID.

## Packet Structure ##
- 1 byte project
- 1 byte class
- 2 bytes (unsigned short) commandId

## Packet Processing ##
Packet IDs and structures can be found in the official SDK: [ArCommands](https://github.com/ARDroneSDK3/libARCommands/tree/master/Xml) 

For the correct project, class and commands, it might be necessary to compile these with the Python script and check the generated **ARCOMMANDS_Ids.h** file in the sources directory.

## Sending Packets ##
The same packet structure is used as mentioned above.
Payload for the packet is specific to the specifications in the command reference XML.

Packets are encapsulated within a frame, with the following constraints:

- Frame type can be chosen by the sender and can be chosen to be either control (preferably no delay) or status requests (delay allowed). For some packets, e.g. emergency landing, an ACK can be requested.
Thesame types can be used as for the frame processing above. When ARNETWORKAL\_FRAME\_TYPE\_DATA\_WITH\_ACK is used, no packets are allowed to be sent before the ACK delay has timed out. (in other words, data from sender is expected before continuing)
The Low-latency frames do not assume any delay. 
ACK types can be resent in case they are timed out. (using the old sequence id)

- Packet ID can be anything defined by the programmer > 2 and < 128 (due to ACK channels). Packets with ID <= 2 are reserved for internal communication (PING & PONG).
- Sequence ID's are simply incremented for each packet.

## Reserved channels ##
- Location / state data (data, no ack required) = 127
- Events (data, ack required) = 126
- Video (low latency, no ack required) = 125

## Default UDP/IP Settings ##
- Sending Port: 54321 
- Receiving Port: 54321
- Default IP: 192.168.42.1

## ACK Frames ##
The ACK packet (always responded for type ARNETWORKAL\_FRAME\_TYPE\_DATA\_WITH\_ACK) uses the ID of the received packet to calculate a new destination ID.
**The frame data only contains the sequence number.**

When received a packet and acknowledging to the drone:

	sendID = channelId + (maxIds / 2)

When received a packet from the drone with an ack, the sequence number it contains is dispatched to the following ID (inverse operation):

	sendID = channelId - (maxIds / 2)

Where ``maxIds`` = 256 (WIFI) and 32 (BLE).
The frame type for these packets is ARNETWORKAL\_FRAME\_TYPE\_ACK (0x01).

## Ping & Pong ##
Ping and Pong channels (ID Ping 0 & Pong 1) use a ARNETWORKAL\_FRAME\_TYPE\_DATA type.
The ping packet only contains the ``timespec`` struct, as defined here: [time.h](http://pubs.opengroup.org/onlinepubs/007908775/xsh/time.h.html)

This depends on the ``clock_gettime = seconds + nanoseconds`` since Epoch. The structure is 2 longs (16 bytes):

- ``long int tv_sec``
This represents the number of whole seconds of elapsed time.

- ``long int tv_nsec``
This is the rest of the elapsed time (a fraction of a second), represented as the number of nanoseconds. It is always less than one billion.

## Video Packets ##
Video data is sent on channel 125.
Video ACK is on channel 13.

A frame is preceded by a video header:

- 1 ushort frameNumber
- 1 byte frameFlags (currently only least signf. bit = flush frame)
- 1 byte fragmentNumber
- 1 byte fragmentsPerFrame

A frame ack is sent using the following information (on a low-latency channel):

- 1 ushort frameNumber
- 1 ulong upper 64 packets bitfield
- 1 ulong lower 64 packets bitfield

Then the rest of the data is H.264 video data for the given frameNum and fragment.

## Discovery Protocol ##
A discovery protocol can be used to discover the ArDrone ports. This is done over TCP 44444.
First, the controller sends the following packet to the drone to configure the controller ports:

	{ "d2c_port": 54321, "controller_name": "toto", "controller_type": "tata" }
Controller name and type can be replaced with your own device. e.g. for a Nexus 4, it is ``Nexus 4`` and ``mako``.

Next, the drone responds with:

	{ "status": 0, "c2d_port": 54321, "arstream_fragment_size": 1000, "arstream_fragment_maximum_number": 128, "arstream_max_ack_interval": 0, "c2d_update_port": 51, "c2d_user_port": 21 }

The connection is terminated by the drone.

## Literal encoding ##
- Strings are always null terminated, UTF-8. There is no length prefix added.
- Enums are encoded as 4 byte integers.

## Notes ##
**All code assumes LITTLE ENDIAN**. When implemented in Java, unsigned types might require a bigger signed equivalent, or can be assumed to never overflow their signed equivalent.
The protocol is composed of frames. These frames are sent over UDP of BLE. In following writeup, UDP/IP is assumed, but this might as well be correct for BLE packets.
