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
        public static Vector3 VectorDirection(this Quaternion rotation, Vector3 point)
        {
            float num1 = rotation.X * 2f;
            float num2 = rotation.Y * 2f;
            float num3 = rotation.Z * 2f;
            float num4 = rotation.X * num1;
            float num5 = rotation.Y * num2;
            float num6 = rotation.Z * num3;
            float num7 = rotation.X * num2;
            float num8 = rotation.X * num3;
            float num9 = rotation.Y * num3;
            float num10 = rotation.W * num1;
            float num11 = rotation.W * num2;
            float num12 = rotation.W * num3;
            Vector3 vector3;
            vector3.X = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.X + ((double)num7 - (double)num12) * (double)point.Y + ((double)num8 + (double)num11) * (double)point.Z);
            vector3.Y = (float)(((double)num7 + (double)num12) * (double)point.X + (1.0 - ((double)num4 + (double)num6)) * (double)point.Y + ((double)num9 - (double)num10) * (double)point.Z);
            vector3.Z = (float)(((double)num8 - (double)num11) * (double)point.X + ((double)num9 + (double)num10) * (double)point.Y + (1.0 - ((double)num4 + (double)num5)) * (double)point.Z);
            return vector3;
        }

        public static Quaternion CalculateRotationQuaternion(Vector3 from, Vector3 to)
        {
            Vector3 currentNormalized = Vector3.Normalize(from);
            Vector3 targetNormalized = Vector3.Normalize(to);

            // Calculate the rotation axis
            Vector3 rotationAxis = Vector3.Cross(currentNormalized, targetNormalized);

            // Calculate the angle between the vectors
            float angle = (float)Math.Acos(Vector3.Dot(currentNormalized, targetNormalized));

            // Create the quaternion from the axis and angle
            Quaternion rotationQuaternion = Quaternion.CreateFromAxisAngle(rotationAxis, angle);

            return rotationQuaternion;
        }

        public static Quaternion LookAt(Vector3 position, Vector3 target)
        {
            var value = Vector3.Normalize(new Vector3(target.X, 0, target.Z) - new Vector3(position.X, 0, position.Z));
            Matrix4x4 viewMatrix = Matrix4x4.CreateLookTo(position, new Vector3(value.X, value.Y, value.Z * -1), Vector3.UnitY);
            return Quaternion.CreateFromRotationMatrix(viewMatrix);
        }
        public static Vector3 QuaternionToEuler(this Quaternion q)
        {
            Vector3 angles = new Vector3();

            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            var degrees = new Vector3()
            {
                X = (float)(180 / Math.PI) * angles.X,
                Y = (float)(180 / Math.PI) * angles.Y,
                Z = (float)(180 / Math.PI) * angles.Z
            };

            return degrees;
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
