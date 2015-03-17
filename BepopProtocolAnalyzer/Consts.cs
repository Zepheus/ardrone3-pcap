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

    public enum ArDrone3PilotingCommand : ushort
    {
        FLATTRIM = 0,
        TAKEOFF,
        PCMD,
        LANDING,
        EMERGENCY,
        NAVIGATEHOME,
        AUTOTAKEOFFMODE,
    }

    public enum ArDrone3AnimationCommand : ushort
    {
        FLIP = 0,
    }

    public enum ArDrone3CameraCommand : ushort
    {
        ARCOMMANDS_ID_ARDRONE3_CAMERA_CMD_ORIENTATION = 0,
    }

    public enum ArDrone3MediaRecordCommand : ushort
    {
        PICTURE = 0,
        VIDEO,
        PICTUREV2,
        VIDEOV2,
    }

    public enum ArDrone3MediaRecordStateCommand : ushort
    {
        PICTURESTATECHANGED = 0,
        VIDEOSTATECHANGED,
        PICTURESTATECHANGEDV2,
        VIDEOSTATECHANGEDV2,
    }

    public enum ArDrone3MediaRecordEventCommand : ushort
    {
        PICTUREEVENTCHANGED = 0,
        VIDEOEVENTCHANGED,
    }

    public enum ArDrone3PilotingStateCommand : ushort
    {
        FLATTRIMCHANGED = 0,
        FLYINGSTATECHANGED,
        ALERTSTATECHANGED,
        NAVIGATEHOMESTATECHANGED,
        POSITIONCHANGED,
        SPEEDCHANGED,
        ATTITUDECHANGED,
        AUTOTAKEOFFMODECHANGED,
        ALTITUDECHANGED,
    }

    public enum ArDrone3NetworkCommand : ushort
    {
        WIFISCAN = 0,
        WIFIAUTHCHANNEL,
    }

    public enum ArDrone3NetworkStateCommand : ushort
    {
        WIFISCANLISTCHANGED = 0,
        ALLWIFISCANCHANGED,
        WIFIAUTHCHANNELLISTCHANGED,
        ALLWIFIAUTHCHANNELCHANGED,
    }

    public enum ArDrone3PilotingSettingCommand : ushort
    {
        MAXALTITUDE = 0,
        MAXTILT,
        ABSOLUTCONTROL,
    }

    public enum ArDrone3PilotingSettingStateCommand : ushort
    {
        MAXALTITUDECHANGED = 0,
        MAXTILTCHANGED,
        ABSOLUTCONTROLCHANGED,
    }

    public enum ArDrone3SpeedSettingsCommand : ushort
    {
        MAXVERTICALSPEED = 0,
        MAXROTATIONSPEED,
        HULLPROTECTION,
        OUTDOOR,
    }

    public enum ArDrone3SpeedSettingsStateCommand : ushort
    {
        MAXVERTICALSPEEDCHANGED = 0,
        MAXROTATIONSPEEDCHANGED,
        HULLPROTECTIONCHANGED,
        OUTDOORCHANGED,
    }

    public enum ArDrone3NetworkSettingsCommand : ushort
    {
        WIFISELECTION = 0,
    }

    public enum ArDrone3NetworkSettingsStateCommand : ushort
    {
        WIFISELECTIONCHANGED = 0,
    }

    public enum ArDrone3SettingsStateCommand : ushort
    {
        PRODUCTMOTORVERSIONLISTCHANGED = 0,
        PRODUCTGPSVERSIONCHANGED,
        MOTORERRORSTATECHANGED,
        MOTORSOFTWAREVERSIONCHANGED,
        MOTORFLIGHTSSTATUSCHANGED,
        MOTORERRORLASTERRORCHANGED,
    }

    public enum ArDrone3PictureSettingCommand : ushort
    {
        PICTUREFORMATSELECTION = 0,
        AUTOWHITEBALANCESELECTION,
        EXPOSITIONSELECTION,
        SATURATIONSELECTION,
        TIMELAPSESELECTION,
        VIDEOAUTORECORDSELECTION,
    }

    public enum ArDrone3PictureSettingStateCommand : ushort
    {
        PICTUREFORMATCHANGED = 0,
        AUTOWHITEBALANCECHANGED,
        EXPOSITIONCHANGED,
        SATURATIONCHANGED,
        TIMELAPSECHANGED,
        VIDEOAUTORECORDCHANGED,
    }

    public enum ArDrone3MediaStreamingCommand : ushort
    {
        VIDEOENABLE = 0,
    }

    public enum ArDrone3MediaStreamingStateCommand : ushort
    {
        VIDEOENABLECHANGED = 0,
    }

    public enum ArDrone3GPSSettingCommand : ushort
    {
        SETHOME = 0,
        RESETHOME,
        SENDCONTROLLERGPS,
    }

    public enum ArDrone3GPSSettingStateCommand : ushort
    {
        HOMECHANGED = 0,
        RESETHOMECHANGED,
        GPSFIXSTATECHANGED,
        GPSUPDATESTATECHANGED,
    }

    public enum ArDrone3CameraStateCommand : ushort
    {
        ORIENTATION = 0,
    }

    public enum ArDrone3AntiFlickeringCommand : ushort
    {
        ELECTRICFREQUENCY = 0,
    }

    public enum ArDrone3DebugVideoCommand : ushort
    {
        ENABLEWOBBLECANCELLATION = 0,
        SYNCANGLESGYROS,
        MANUALWHITEBALANCE,
    }

    public enum ArDrone3DebugBatterySettingsCommand : ushort
    {
        USEDRONE2BATTERY = 0,
    }

    public enum ArDrone3DebugBatterySettingsStateCommand : ushort
    {
        USEDRONE2BATTERYCHANGED = 0
    }

    public enum ArDrone3DebugGpsStateCommand : ushort
    {
        NBSATELLITECHANGED = 0
    }

    public enum CommonCommonStateCommand : ushort
    {
        ALLSTATESCHANGED = 0,
        BATTERYSTATECHANGED,
        MASSSTORAGESTATELISTCHANGED,
        MASSSTORAGEINFOSTATELISTCHANGED,
        CURRENTDATECHANGED,
        CURRENTTIMECHANGED,
        MASSSTORAGEINFOREMAININGLISTCHANGED,
        WIFISIGNALCHANGED,
        SENSORSSTATESLISTCHANGED,
    }
}
