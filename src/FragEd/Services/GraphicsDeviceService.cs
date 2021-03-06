﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace FragEd.Services
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        // Singleton device service instance.
        static GraphicsDeviceService _singletonInstance;

        // Keep track of how many controls are sharing the singletonInstance.
        static int _referenceCount;

        private GraphicsDevice _graphicsDevice;

        // IGraphicsDeviceService events.
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        GraphicsDeviceService( IntPtr windowHandle, int width, int height )
        {
            _graphicsDevice = new GraphicsDevice();

            _graphicsDevice.PresentationParameters.DeviceWindowHandle = windowHandle;
            _graphicsDevice.PresentationParameters.BackBufferWidth = Math.Max( width, 1 );
            _graphicsDevice.PresentationParameters.BackBufferHeight = Math.Max( height, 1 );

            if( DeviceCreated != null )
            {
                DeviceCreated( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef( IntPtr windowHandle, int width, int height )
        {
            // Increment the "how many controls sharing the device" reference count.
            if( Interlocked.Increment( ref _referenceCount ) == 1 )
            {
                // If this is the first control to start using the
                // device, we must create the singleton instance.
                _singletonInstance = new GraphicsDeviceService( windowHandle, width, height );
            }

            return _singletonInstance;
        }


        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release( bool disposing = true )
        {
            // Decrement the "how many controls sharing the device" reference count.
            if( Interlocked.Decrement( ref _referenceCount ) == 0 )
            {
                // If this is the last control to finish using the
                // device, we should dispose the singleton instance.
                if( disposing )
                {
                    if( DeviceDisposing != null )
                        DeviceDisposing( this, EventArgs.Empty );

                    try
                    {
                        _graphicsDevice.Dispose();    
                    } catch {} // yeah, i'm an awful person.
                    
                }

                _graphicsDevice = null;
            }
        }

        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice( int width, int height )
        {

        }
    }
}
