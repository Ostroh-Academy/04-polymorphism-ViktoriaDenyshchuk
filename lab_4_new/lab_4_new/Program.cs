﻿using System;

namespace TransformationApp
{
    public abstract class TransformationFactory
    {
        public abstract PlaneTransformation CreateTransformation();
    }
    public class PlaneTransformationFactory : TransformationFactory
    {
        public override PlaneTransformation CreateTransformation()
        {
            return new PlaneTransformation(1, 2, 3, 4, 5, 6);
        }
    }

    public class SpaceTransformationFactory : TransformationFactory
    {
        public override PlaneTransformation CreateTransformation()
        {
            return new SpaceTransformation(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
    public class PlaneTransformation 
    {
        protected double a11, a12, a13;
        protected double a21, a22, a23;

        public PlaneTransformation(double a11, double a12, double a13, double a21, double a22, double a23)
        {
            this.a11 = a11;
            this.a12 = a12;
            this.a13 = a13;
            this.a21 = a21;
            this.a22 = a22;
            this.a23 = a23;
        }

        public virtual void PrintCoefficients()
        {
            Console.WriteLine($"\na11 = {a11}, a12 = {a12}, a13 = {a13}");
            Console.WriteLine($"a21 = {a21}, a22 = {a22}, a23 = {a23}");
        }

        public virtual (double x, double y) TransformPoint(double x, double y)
        {
            double xPrime = a11 * x + a12 * y + a13;
            double yPrime = a21 * x + a22 * y + a23;
            return (xPrime, yPrime);
        }
    }

    public class SpaceTransformation : PlaneTransformation
    {
        private double a31, a32, a33, a34;

        public SpaceTransformation(double a11, double a12, double a13, double a21, double a22, double a23, double a31, double a32, double a33, double a34)
            : base(a11, a12, a13, a21, a22, a23)
        {
            this.a31 = a31;
            this.a32 = a32;
            this.a33 = a33;
            this.a34 = a34;
        }

        public override void PrintCoefficients()
        {
            base.PrintCoefficients();
            Console.WriteLine($"a31 = {a31}, a32 = {a32}, a33 = {a33}, a34 = {a34}");
        }

        public (double x, double y, double z) TransformPoint3D(double x, double y, double z)
        {
            (double x, double y) pointInPlane = base.TransformPoint(x, y);
            double xPrime = pointInPlane.x + a34;
            double yPrime = pointInPlane.y + a34;
            double zPrime = a31 * x + a32 * y + a33 * z + a34;
            return (xPrime, yPrime, zPrime);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TransformationFactory factory;
            char userChoose;
            Console.WriteLine("Виберіть '1' для роботи з x, y");
            Console.WriteLine("Виберіть інше число для роботи з x, y, z");
            userChoose = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (userChoose == '1')
            {
                factory = new PlaneTransformationFactory();
            }
            else
            {
                factory = new SpaceTransformationFactory();
            }
            PlaneTransformation transformation = factory.CreateTransformation();
            transformation.PrintCoefficients();

            if (transformation is SpaceTransformation spaceTransformation)
            {
                Console.WriteLine("Введіть координати точки (x, y, z): ");
                double x = Convert.ToDouble(Console.ReadLine());
                double y = Convert.ToDouble(Console.ReadLine());
                double z = Convert.ToDouble(Console.ReadLine());
                var transformedPoint3D = spaceTransformation.TransformPoint3D(x, y, z);
                Console.WriteLine($"Образ точки ({x}, {y}, {z}) -> ({transformedPoint3D.x}, {transformedPoint3D.y}, {transformedPoint3D.z})");
            }
            else
            {
                Console.WriteLine("Введіть координати точки (x, y): ");
                double x = Convert.ToDouble(Console.ReadLine());
                double y = Convert.ToDouble(Console.ReadLine());
                var transformedPoint = transformation.TransformPoint(x, y);
                Console.WriteLine($"Образ точки ({x}, {y}) -> ({transformedPoint.x}, {transformedPoint.y})");
            }


        }
    }
}
