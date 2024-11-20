﻿using System.Numerics;

namespace Lab1.Models;

public enum AnatomicPlane
{
    Saggital,
    Axial,
    Coronal
}

public static class AnatomicPlaneRelations
{
    public static Matrix4x4 AddDepth(AnatomicPlane sourcePlane, AnatomicPlane targetPlane, float depth) =>
        (sourcePlane, targetPlane) switch
        {
            (AnatomicPlane.Axial, AnatomicPlane.Axial) => Matrix4x4.CreateTranslation(0,0, depth),
            //(var plane1, var plane2) when plane1 == plane2 => Matrix4x4.Identity,
            (AnatomicPlane.Axial, AnatomicPlane.Saggital) => Matrix4x4.CreateTranslation(0, 0, depth),

            (AnatomicPlane.Axial, AnatomicPlane.Coronal) => Matrix4x4.CreateTranslation(0, 0, depth),

            _ => throw new ArgumentException("Either source plane or target plane is not valid.")
        };

    public static Matrix4x4 PlaneTransform(AnatomicPlane sourcePlane, AnatomicPlane targetPlane)
    {
        return (sourcePlane, targetPlane) switch
        {
            (var plane1, var plane2) when plane1 == plane2 => Matrix4x4.Identity,

            (AnatomicPlane.Axial, AnatomicPlane.Saggital) => Matrix4x4.CreateRotationY(float.Pi/2),

            (AnatomicPlane.Axial, AnatomicPlane.Coronal) => Matrix4x4.CreateRotationX(float.Pi/2),

            (AnatomicPlane.Saggital, AnatomicPlane.Axial) => Matrix4x4.CreateRotationY(-float.Pi / 2f),

            (AnatomicPlane.Saggital, AnatomicPlane.Coronal) => Matrix4x4.CreateRotationZ(float.Pi / 2f),

            (AnatomicPlane.Coronal, AnatomicPlane.Axial) => Matrix4x4.CreateRotationX(-float.Pi / 2f),

            (AnatomicPlane.Coronal, AnatomicPlane.Saggital) => Matrix4x4.CreateRotationZ(-float.Pi / 2f),

            _ => throw new ArgumentException("Either source plane or target plane is not valid.")
        };
    }
}