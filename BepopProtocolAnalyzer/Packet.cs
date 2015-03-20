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


        public static string GetPacketClass(PacketType type, byte c)
        {
            switch (type)
            {
                case PacketType.COMMON:
                    return ((CommonPacketClass)c).ToString();
                case PacketType.ARDRONE3:
                    return ((Ardrone3PacketClass)c).ToString();
                case PacketType.ARDRONE3DEBUG:
                    return ((Ardrone3DebugClass)c).ToString();
                default:
                    return c.ToString();
            }
        }

        public static string GetPacketCommand(PacketType type, byte cl, ushort cmd)
        {
            switch (type)
            {
                case PacketType.ARDRONE3:
                    switch ((Ardrone3PacketClass)cl)
                    {
                        case Ardrone3PacketClass.PILOTING:
                            return ((ArDrone3PilotingCommand)cmd).ToString();
                        case Ardrone3PacketClass.ANIMATIONS:
                            return ((ArDrone3AnimationCommand)cmd).ToString();
                        case Ardrone3PacketClass.CAMERA:
                            return ((ArDrone3CameraCommand)cmd).ToString();
                        case Ardrone3PacketClass.MEDIARECORD:
                            return ((ArDrone3MediaRecordCommand)cmd).ToString();
                        case Ardrone3PacketClass.MEDIARECORDSTATE:
                            return ((ArDrone3MediaRecordStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.MEDIARECORDEVENT:
                            return ((ArDrone3MediaRecordEventCommand)cmd).ToString();
                        case Ardrone3PacketClass.PILOTINGSTATE:
                            return ((ArDrone3PilotingStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.NETWORK:
                            return ((ArDrone3NetworkCommand)cmd).ToString();
                        case Ardrone3PacketClass.NETWORKSTATE:
                            return ((ArDrone3NetworkStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.PILOTINGSETTINGS:
                            return ((ArDrone3PilotingSettingCommand)cmd).ToString();
                        case Ardrone3PacketClass.PILOTINGSETTINGSSTATE:
                            return ((ArDrone3PilotingSettingStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.SPEEDSETTINGS:
                            return ((ArDrone3SpeedSettingsCommand)cmd).ToString();
                        case Ardrone3PacketClass.SPEEDSETTINGSSTATE:
                            return ((ArDrone3SpeedSettingsStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.NETWORKSETTINGS:
                            return ((ArDrone3NetworkSettingsCommand)cmd).ToString();
                        case Ardrone3PacketClass.NETWORKSETTINGSSTATE:
                            return ((ArDrone3NetworkSettingsStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.SETTINGSSTATE:
                            return ((ArDrone3SettingsStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.PICTURESETTINGS:
                            return ((ArDrone3PictureSettingCommand)cmd).ToString();
                        case Ardrone3PacketClass.PICTURESETTINGSSTATE:
                            return ((ArDrone3PictureSettingStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.MEDIASTREAMING:
                            return ((ArDrone3MediaStreamingCommand)cmd).ToString();
                        case Ardrone3PacketClass.MEDIASTREAMINGSTATE:
                            return ((ArDrone3MediaStreamingStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.GPSSETTINGS:
                            return ((ArDrone3GPSSettingCommand)cmd).ToString();
                        case Ardrone3PacketClass.GPSSETTINGSSTATE:
                            return ((ArDrone3GPSSettingStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.CAMERASTATE:
                            return ((ArDrone3CameraStateCommand)cmd).ToString();
                        case Ardrone3PacketClass.ANTIFLICKERING:
                            return ((ArDrone3AntiFlickeringCommand)cmd).ToString();
                        default:
                            return cmd.ToString();
                    }
                case PacketType.ARDRONE3DEBUG:
                    switch ((Ardrone3DebugClass)cl)
                    {
                        case Ardrone3DebugClass.DEBUG_CLASS_BATTERYDEBUGSETTINGS:
                            return ((ArDrone3DebugBatterySettingsCommand)cmd).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_BATTERYDEBUGSETTINGSSTATE:
                            return ((ArDrone3DebugBatterySettingsStateCommand)cmd).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_GPSDEBUGSTATE:
                            return ((ArDrone3DebugGpsStateCommand)cmd).ToString();
                        case Ardrone3DebugClass.DEBUG_CLASS_VIDEO:
                            return ((ArDrone3DebugVideoCommand)cmd).ToString();
                        default:
                            return cmd.ToString();
                    }
                case PacketType.COMMON:
                    switch ((CommonPacketClass) cl)
                    {
                        case CommonPacketClass.COMMONSTATE:
                            return ((CommonCommonStateCommand) cmd).ToString();
                        case CommonPacketClass.SETTINGSSTATE:
                            return ((CommonSettingsStateCommand) cmd).ToString();
                        default:
                            return cmd.ToString();
                    }
                default:
                    return cmd.ToString();
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
