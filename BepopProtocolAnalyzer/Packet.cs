using System;

namespace BepopProtocolAnalyzer
{
    public class Packet
    {


        public byte[] Data { get; private set; }
        public PacketType Project { get; private set; }
        public byte Class { get; private set; }
        public ushort Command { get; private set; }

        public Packet(PacketType project, byte clas, ushort command, byte[] data)
        {
            Project = project;
            Class = clas;
            Command = command;
            Data = data;
        }

        public static string GetPacketClass(Packet packet)
        {
            switch (packet.Project)
            {
                case PacketType.COMMON:
                    return ((CommonPacketClass)packet.Class).ToString();
                case PacketType.ARDRONE3:
                    return ((Ardrone3PacketClass)packet.Class).ToString();
                case PacketType.ARDRONE3DEBUG:
                    return ((Ardrone3DebugClass)packet.Class).ToString();
                default:
                    return packet.Class.ToString();
            }
        }

        public static string GetPacketCommand(Packet packet)
        {
            switch (packet.Project)
            {
                case PacketType.ARDRONE3:
                    switch ((Ardrone3PacketClass)packet.Class)
                    {
                        case Ardrone3PacketClass.PILOTING:
                            return ((ArDrone3PilotingCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.ANIMATIONS:
                            return ((ArDrone3AnimationCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.CAMERA:
                            return ((ArDrone3CameraCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.MEDIARECORD:
                            return ((ArDrone3MediaRecordCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.MEDIARECORDSTATE:
                            return ((ArDrone3MediaRecordStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.MEDIARECORDEVENT:
                            return ((ArDrone3MediaRecordEventCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.PILOTINGSTATE:
                            return ((ArDrone3PilotingStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.NETWORK:
                            return ((ArDrone3NetworkCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.NETWORKSTATE:
                            return ((ArDrone3NetworkStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.PILOTINGSETTINGS:
                            return ((ArDrone3PilotingSettingCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.PILOTINGSETTINGSSTATE:
                            return ((ArDrone3PilotingSettingStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.SPEEDSETTINGS:
                            return ((ArDrone3SpeedSettingsCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.SPEEDSETTINGSSTATE:
                            return ((ArDrone3SpeedSettingsStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.NETWORKSETTINGS:
                            return ((ArDrone3NetworkSettingsCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.NETWORKSETTINGSSTATE:
                            return ((ArDrone3NetworkSettingsStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.SETTINGSSTATE:
                            return ((ArDrone3SettingsStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.PICTURESETTINGS:
                            return ((ArDrone3PictureSettingCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.PICTURESETTINGSSTATE:
                            return ((ArDrone3PictureSettingStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.MEDIASTREAMING:
                            return ((ArDrone3MediaStreamingCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.MEDIASTREAMINGSTATE:
                            return ((ArDrone3MediaStreamingStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.GPSSETTINGS:
                            return ((ArDrone3GPSSettingCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.GPSSETTINGSSTATE:
                            return ((ArDrone3GPSSettingStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.CAMERASTATE:
                            return ((ArDrone3CameraStateCommand)packet.Command).ToString();
                        case Ardrone3PacketClass.ANTIFLICKERING:
                            return ((ArDrone3AntiFlickeringCommand)packet.Command).ToString();
                        default:
                            return packet.Command.ToString();
                    }
                case PacketType.ARDRONE3DEBUG:
                    switch ((Ardrone3DebugClass)packet.Class)
                    {
                        case Ardrone3DebugClass.DEBUG_CLASS_BATTERYDEBUGSETTINGS:
                            return ((ArDrone3DebugBatterySettingsCommand)packet.Command).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_BATTERYDEBUGSETTINGSSTATE:
                            return ((ArDrone3DebugBatterySettingsStateCommand)packet.Command).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_GPSDEBUGSTATE:
                            return ((ArDrone3DebugGpsStateCommand)packet.Command).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_VIDEO:
                            return ((ArDrone3DebugVideoCommand)packet.Command).ToString();
                        default:
                            return packet.Command.ToString();
                    }
                default:
                    return packet.Command.ToString();
            }
        }

        public static Packet Parse(byte[] data)
        {
            byte p = data[0];
            byte c = data[1];
            ushort command = 0;
            command = data[2];
            command |= (ushort)(data[3] << 8);

            byte[] payload = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, payload, 0, data.Length - 4);
            return new Packet((PacketType)p, c, command, payload);
        }
    }
}
