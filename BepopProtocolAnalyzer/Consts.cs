using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public enum FrameType : byte
    {
        ACK = 1,
        DATA = 2,
        DATA_LL = 3,
        DATA_W_ACK = 4
    }

    public enum PacketType : byte
    {
        ARDRONE3 = 1,
        ARDRONE3DEBUG = 129,
        JUMPINGSUMO = 3,
        JUMPINGSUMODEBUG = 131,
        MINIDRONE = 2,
        MINIDRONEDEBUG = 130,
        SKYCONTROLLER = 4,
        SKYCONTROLLERDEBUG = 132,
        COMMON = 0,
        COMMONDEBUG = 128,

    }

    public enum Ardrone3PacketClass : byte
    {
        PILOTING = 0,
        ANIMATIONS = 5,
        CAMERA = 1,
        MEDIARECORD = 7,
        MEDIARECORDSTATE = 8,
        MEDIARECORDEVENT = 3,
        PILOTINGSTATE = 4,
        NETWORK = 13,
        NETWORKSTATE = 14,
        PILOTINGSETTINGS = 2,
        PILOTINGSETTINGSSTATE = 6,
        SPEEDSETTINGS = 11,
        SPEEDSETTINGSSTATE = 12,
        NETWORKSETTINGS = 9,
        NETWORKSETTINGSSTATE = 10,
        SETTINGS = 15,
        SETTINGSSTATE = 16,
        DIRECTORMODE = 17,
        DIRECTORMODESTATE = 18,
        PICTURESETTINGS = 19,
        PICTURESETTINGSSTATE = 20,
        MEDIASTREAMING = 21,
        MEDIASTREAMINGSTATE = 22,
        GPSSETTINGS = 23,
        GPSSETTINGSSTATE = 24,
        CAMERASTATE = 25,
        ANTIFLICKERING = 29,
    }

    public enum Ardrone3DebugClass : byte
    {
        DEBUG_CLASS_VIDEO = 0,
        DEBUG_CLASS_BATTERYDEBUGSETTINGS = 1,
        DEBUG_CLASS_BATTERYDEBUGSETTINGSSTATE = 2,
        DEBUG_CLASS_GPSDEBUGSTATE = 3,
    }

    public enum CommonPacketClass : byte
    {
        NETWORK = 0,
        NETWORKEVENT = 1,
        SETTINGS = 2,
        SETTINGSSTATE = 3,
        COMMON = 4,
        COMMONSTATE = 5,
        OVERHEAT = 6,
        OVERHEATSTATE = 7,
        CONTROLLERSTATE = 8,
        WIFISETTINGS = 9,
        WIFISETTINGSSTATE = 10,
        MAVLINK = 11,
        MAVLINKSTATE = 12,
        CALIBRATION = 13,
        CALIBRATIONSTATE = 14,
        CAMERASETTINGSSTATE = 15,
        GPS = 16,
        FLIGHTPLANSTATE = 17,
        FLIGHTPLANEVENT = 19,
        ARLIBSVERSIONSSTATE = 18
    }
}
