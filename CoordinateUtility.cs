using System;
using System.Collections.Generic;
using System.Numerics;

namespace AQuestReborn
{
    public static class CoordinateUtility
    {
        public static string[] FillNewList(int count, string phrase)
        {
            List<string> values = new List<string>();
            for (int i = 0; i < count; i++)
            {
                values.Add(phrase + " " + i);
            }
            return values.ToArray();
        }
        public static float ConvertRadiansToDegrees(float radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (float)degrees;
        }
        public static float ConvertDegreesToRadians(float degrees)
        {
            double radians = degrees * (Math.PI / 180);
            return (float)radians;
        }
        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return degrees;
        }
        public static double ConvertDegreesToRadians(double degrees)
        {
            double radians = degrees * (Math.PI / 180);
            return radians;
        }

        public static Quaternion ToQuaternion(Vector3 vector3) // roll (x), pitch (y), yaw (z), angles are in radians
        {
            return ToQuaternion(vector3.X, vector3.Y, vector3.Z);
        }
        public static Vector3 ToEuler(Quaternion q) // Returns the XYZ in ZXY
        {
            Vector3 angles;

            angles.X = (float)Math.Atan2(2 * (q.W * q.X + q.Y * q.Z), 1 - 2 * (q.X * q.X + q.Y * q.Y));
            if (Math.Abs(2 * (q.W * q.Y - q.Z * q.X)) >= 1) angles.Y = (float)Math.CopySign(Math.PI / 2, 2 * (q.W * q.Y - q.Z * q.X));
            else angles.Y = (float)Math.Asin(2 * (q.W * q.Y - q.Z * q.X));
            angles.Z = (float)Math.Atan2(2 * (q.W * q.Z + q.X * q.Y), 1 - 2 * (q.Y * q.Y + q.Z * q.Z));

            return new Vector3()
            {
                X = (float)(180 / Math.PI) * angles.X,
                Y = (float)(180 / Math.PI) * angles.Y,
                Z = (float)(180 / Math.PI) * angles.Z
            };
        }
        public static Quaternion ToQuaternion(double x, double y, double z) // roll (x), pitch (y), yaw (z), angles are in radians
        {
            // Abbreviations for the various angular functions
            double roll, pitch, yaw;

            roll = ConvertDegreesToRadians(x);
            pitch = ConvertDegreesToRadians(y);
            yaw = ConvertDegreesToRadians(z);

            double cr = Math.Cos(roll * 0.5);
            double sr = Math.Sin(roll * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double cy = Math.Cos(yaw * 0.5);
            double sy = Math.Sin(yaw * 0.5);

            Quaternion q = new Quaternion();
            q.W = (float)(cr * cp * cy + sr * sp * sy);
            q.X = (float)(sr * cp * cy - cr * sp * sy);
            q.Y = (float)(cr * sp * cy + sr * cp * sy);
            q.Z = (float)(cr * cp * sy - sr * sp * cy);

            return q;
        }
    }
}
