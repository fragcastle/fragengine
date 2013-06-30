using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FragEd.Services;
using FragEngine.Entities;
using FragEngine.Mapping;
using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework.Graphics;
using OpenTK;
using FragEngine;

namespace FragEd.Controls
{
    using XnaColor = Microsoft.Xna.Framework.Color;

    public abstract class GraphicsDeviceControl : GLControl
    {

        private readonly bool _designMode;

        protected GraphicsDeviceService _deviceService;
        protected Stopwatch _timer;

        private readonly ServiceContainer _services = new ServiceContainer();

        private Camera _camera;
        private SpriteBatch _spriteBatch;

        public Form MainForm { get; internal set; }

        public GraphicsDevice GraphicsDevice
        {
            get { return _deviceService.GraphicsDevice; }
        }

        public GraphicsDeviceService GraphicsDeviceService
        {
            get { return _deviceService; }
        }

        public ServiceContainer Services
        {
            get { return _services; }
        }

        public Camera Camera
        {
            get { return _camera; }
        }

        public event EventHandler<EventArgs> ControlInitialized;
        public event EventHandler<EventArgs> ControlInitializing;

        protected GraphicsDeviceControl()
        {
            _designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        protected override void OnCreateControl()
        {
            if( !DesignMode )
            {
                _deviceService = GraphicsDeviceService.AddRef( Handle, ClientSize.Width, ClientSize.Height );

                _services.AddService<IGraphicsDeviceService>( _deviceService );

                if( !ServiceInjector.Has<IGraphicsDeviceService>() )
                    ServiceInjector.Add<IGraphicsDeviceService>( _deviceService );

                if( !ServiceInjector.Has<GraphicsDevice>() )
                    ServiceInjector.Add( _deviceService.GraphicsDevice );

                if( !ServiceInjector.Has<IEntityService>() )
                    ServiceInjector.Add<IEntityService>( new EntityService() );

                if( !ServiceInjector.Has<ICollisionService>() )
                    ServiceInjector.Add<ICollisionService>( new CollisionService() );

                _camera = new Camera( _deviceService.GraphicsDevice.Viewport );

                if( !ServiceInjector.Has<Camera>() )
                    ServiceInjector.Add( _camera );

                if( ControlInitializing != null )
                {
                    ControlInitializing( this, EventArgs.Empty );
                }

                // Start the animation timer.
                _timer = Stopwatch.StartNew();

                Initialize();

                if( ControlInitialized != null )
                {
                    ControlInitialized( this, EventArgs.Empty );
                }

                Application.Idle += ( o, args ) => Invalidate( true );
            }
        }

        protected override void Dispose( bool disposing )
        {
            if( _deviceService != null )
            {
                try
                {
                    _deviceService.Release();
                }
                catch { }

                _deviceService = null;
            }

            base.Dispose( disposing );
        }

        protected new bool DesignMode
        {
            get { return _designMode; }
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            string beginDrawError = BeginDraw();

            if( string.IsNullOrEmpty( beginDrawError ) )
            {

                GraphicsDevice.Clear( XnaColor.Transparent );

                // create a new sprite batch
                if( _spriteBatch == null || _spriteBatch.GraphicsDevice != GraphicsDevice )
                {
                    _spriteBatch = new SpriteBatch( GraphicsDevice );
                }

                Draw( _spriteBatch );
                EndDraw();
            }
            else
            {
                PaintUsingSystemDrawing( e.Graphics, GetType().ToString() );
            }
        }

        private string BeginDraw()
        {
            if( _deviceService == null )
            {
                return Text + "\n\n" + GetType();
            }

            string deviceResetError = HandleDeviceReset();

            if( !string.IsNullOrEmpty( deviceResetError ) )
            {
                return deviceResetError;
            }

            GLControl control = GLControl.FromHandle( _deviceService.GraphicsDevice.PresentationParameters.DeviceWindowHandle ) as GLControl;
            if( control != null )
            {
                control.Context.MakeCurrent( WindowInfo );
                _deviceService.GraphicsDevice.PresentationParameters.BackBufferHeight = ClientSize.Height;
                _deviceService.GraphicsDevice.PresentationParameters.BackBufferWidth = ClientSize.Width;
            }

            Viewport viewport = new Viewport();

            viewport.X = 0;
            viewport.Y = 0;

            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if( GraphicsDevice.Viewport.Equals( viewport ) == false )
            {
                GraphicsDevice.Viewport = viewport;
                _camera = new Camera( viewport );

                ServiceInjector.Add( _camera );
            }


            return null;
        }

        private void EndDraw()
        {
            try
            {
                SwapBuffers();
            }
            catch
            {
            }
        }

        private string HandleDeviceReset()
        {
            bool needsReset = false;

            switch( GraphicsDevice.GraphicsDeviceStatus )
            {
                case GraphicsDeviceStatus.Lost:
                return "Graphics device lost";

                case GraphicsDeviceStatus.NotReset:
                needsReset = true;
                break;

                default:
                PresentationParameters pp = GraphicsDevice.PresentationParameters;
                needsReset = ( ClientSize.Width > pp.BackBufferWidth ) || ( ClientSize.Height > pp.BackBufferHeight );
                break;
            }

            if( needsReset )
            {
                try
                {
                    _deviceService.ResetDevice( ClientSize.Width, ClientSize.Height );
                }
                catch( Exception e )
                {
                    return "Graphics device reset failed\n\n" + e;
                }
            }

            return null;
        }

        protected virtual void PaintUsingSystemDrawing( Graphics graphics, string text )
        {
            graphics.Clear( Color.Black );

            using( Brush brush = new SolidBrush( Color.White ) )
            {
                using( var format = new StringFormat() )
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    graphics.DrawString( text, Font, brush, ClientRectangle, format );
                }
            }
        }

        protected override void OnPaintBackground( PaintEventArgs pevent )
        {
        }

        protected virtual void Initialize()
        {

        }
        protected abstract void Draw( SpriteBatch spriteBatch );

        public XnaColor GetPixel( int x, int y )
        {
            var target = new RenderTarget2D( GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None );

            GraphicsDevice.SetRenderTarget( target );
            HandleDeviceReset();

            Draw(_spriteBatch);

            GraphicsDevice.SetRenderTarget( null );

            var data = new XnaColor[ GraphicsDevice.Viewport.Width * GraphicsDevice.Viewport.Height ];
            target.GetData( data );

            return data[ GraphicsDevice.Viewport.Width * y + x ];
        }
    }
}
