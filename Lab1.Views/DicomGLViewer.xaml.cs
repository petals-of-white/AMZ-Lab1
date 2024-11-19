﻿using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Lab1.Models;
using Lab1.ViewModels;
using Lab1.Views.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Wpf;

namespace Lab1.Views;

public partial class DicomGLViewer : UserControl
{
    private (float, float, float, float) color = (Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared.NextSingle(), 1);
    private DicomScene? glState;
    private DicomViewModel viewModel = new();

    public DicomGLViewer()
    {
        InitializeComponent();
        ViewModel = new();
    }

    public DicomViewModel ViewModel
    {
        get => viewModel;

        set
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;

            value.PropertyChanged += ViewModel_PropertyChanged;
            DataContext = value;
            viewModel = value;
        }
    }

    public IGraphicsContext InitOpenGL(GLWpfControlSettings settings)
    {
        openTkControl.Start(settings);

        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);

        GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
        {
            Debug.WriteLine($"OpenGL Debug: {Marshal.PtrToStringAnsi(message)}");
        }, IntPtr.Zero);

        (float r, float g, float b, float a) = color;

        GL.ClearColor(r, g, b, a);

        return openTkControl.Context!;
    }

    public void LoadScene(DicomScene? dicomScene = null)
    {
        glState = dicomScene is null ? new() : dicomScene;
    }

    //public void Init(GLWpfControlSettings settings)
    //{
    //    openTkControl.Start(settings);

    //    openTkControl.Context?.MakeCurrent();
    //    GL.Enable(EnableCap.DebugOutput);
    //    GL.Enable(EnableCap.DebugOutputSynchronous);

    //    GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
    //    {
    //        Debug.WriteLine($"OpenGL Debug: {Marshal.PtrToStringAnsi(message)}");
    //    }, IntPtr.Zero);

    //    (float r, float g, float b, float a) = color;

    //    glState = new DicomScene();

    //    GL.ClearColor(r, g, b, a);
    //}

    private void DisplayRegionClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (viewModel.DicomManager is not null && viewModel.SelectedROI is not null)
        {
            var control = (UIElement) sender;
            var coords = e.GetPosition(control);
            viewModel.SetPointCommand.Execute(new PointF((float) coords.X, (float) coords.Y));
        }
    }

    private void OpenTkControl_Render(TimeSpan obj)
    {
        openTkControl.Context?.MakeCurrent();
        GL.Clear(ClearBufferMask.ColorBufferBit);

        //if (ViewModel.CurrentPlane == AnatomicPlane.Coronal)
        //{
        //    GL.Begin(PrimitiveType.Triangles);
        //    GL.Color3(Color.Red);
        //    GL.Vertex3(-0.5f, -0.5f, 0.0f);
        //    GL.Vertex3(0.5f, -0.5f, 0.0f);
        //    GL.Vertex3(0.0f, 0.5f, 0.5f);
        //    GL.End();
        //}
        //else

        glState?.DrawVertices(viewModel.CurrentPlane, viewModel.CurrentSlice);
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.DicomManager) && viewModel.DicomManager is DicomManager dicom)
        {
            glState?.LoadDicomTexture(dicom);
        }
    }
}