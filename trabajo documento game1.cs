//game1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2; // Mantenemos tu alias

namespace Project1
{
    public enum EstadoJuego
    {
        IngresandoNombre,
        MenuPrincipal,
        Jugando
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // CORREGIDO: Sin el "new"
        Vector2 posicionCamara = Vector2.Zero;

        float velocidadCamara = 20;

        string fechaActual = DateTime.Now.ToShortDateString();

        private Partida partidaActual;
        private EstadoJuego estadoActual = EstadoJuego.IngresandoNombre;
        string NombreRey = "";

        SpriteFont FuenteJuego;

        Texture2D texturafondoMenu;
        Sprite spritefondoMenu;

        Texture2D texturaImperio;
        Sprite spriteImperio;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Window.TextInput += (sender, e) =>
            {
                MouseState estadoMouse = Mouse.GetState();

                if (estadoMouse.LeftButton == ButtonState.Pressed)
                {
                    // CORREGIDO: Lógica de Substring para borrar el último carácter
                    if (e.Key == Keys.Back && NombreRey.Length > 0)
                    {
                        NombreRey = NombreRey.Substring(0, NombreRey.Length - 1);
                    }
                    else if (e.Key == Keys.Enter && NombreRey.Length > 0)
                    {
                        estadoActual = EstadoJuego.MenuPrincipal;
                    }
                    else if (e.Key != Keys.Enter && e.Key != Keys.Back)
                    {
                        NombreRey += e.Character; // CORREGIDO: Usar e.Character en vez de e.KeyChar
                    }
                }
            }; // CORREGIDO: Punto y coma agregado aquí
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // CORREGIDO: Dejamos una sola carga de la fuente
            FuenteJuego = Content.Load<SpriteFont>("FuenteJuego");

            texturafondoMenu = Content.Load<Texture2D>("Menu");
            spritefondoMenu = new Sprite(texturafondoMenu, Vector2.Zero);

            texturaImperio = Content.Load<Texture2D>("Imperio");
            spriteImperio = new Sprite(texturaImperio, Vector2.Zero);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState estadoMouse = Mouse.GetState();

            if (estadoActual == EstadoJuego.MenuPrincipal) { ManejoMenu(estadoMouse); }
            else if (estadoActual == EstadoJuego.Jugando) { ManejoJuego(estadoMouse); }

            base.Update(gameTime);
        }

        public void ManejoJuego(MouseState estadoMouse)
        {
            KeyboardState tecladoActual = Keyboard.GetState();

            if (tecladoActual.IsKeyDown(Keys.W)) { posicionCamara.Y += velocidadCamara; }
            if (tecladoActual.IsKeyDown(Keys.S)) { posicionCamara.Y -= velocidadCamara; }
            if (tecladoActual.IsKeyDown(Keys.A)) { posicionCamara.X -= velocidadCamara; }
            if (tecladoActual.IsKeyDown(Keys.D)) { posicionCamara.X += velocidadCamara; }
        }

        public void ManejoMenu(MouseState estadoMouse)
        {
            if (estadoMouse.LeftButton == ButtonState.Pressed)
            {
                if (estadoMouse.X >= 100 && estadoMouse.X <= 300 && estadoMouse.Y >= 200 && estadoMouse.Y <= 250)
                {
                    Jugador nuevoJugador = new Jugador(NombreRey, 100, 50, 200);
                    partidaActual = new Partida("Nueva Partida", 1, nuevoJugador);
                    estadoActual = EstadoJuego.Jugando;
                }
                else if (estadoMouse.X >= 100 && estadoMouse.X <= 300 && estadoMouse.Y >= 340 && estadoMouse.Y <= 400)
                {
                    Exit();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (estadoActual == EstadoJuego.IngresandoNombre)
            {
                spritefondoMenu.Draw(_spriteBatch);
                _spriteBatch.DrawString(FuenteJuego, NombreRey, new Vector2(200, 250), Color.White);
            }
            else if (estadoActual == EstadoJuego.MenuPrincipal)
            {
                spritefondoMenu.Draw(_spriteBatch);
                _spriteBatch.DrawString(FuenteJuego, "NUEVA PARTIDA", new Vector2(100, 200), Color.Yellow);
                _spriteBatch.DrawString(FuenteJuego, "CARGAR PARTIDA", new Vector2(100, 270), Color.White);
                _spriteBatch.DrawString(FuenteJuego, "SALIR", new Vector2(100, 340), Color.Red);
            }
            else if (estadoActual == EstadoJuego.Jugando)
            {
                // Cerramos el batch inicial para abrir el batch con la matriz de la cámara
                _spriteBatch.End();

                _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(posicionCamara, 0)));
                spriteImperio.Draw(_spriteBatch);
                _spriteBatch.End();

                // Volvemos a abrir un batch normal para la interfaz de usuario (HUD) estática
                _spriteBatch.Begin();
                // NOTA: Asegúrate de que partidaActual.datosJugador.Oro (etc) sean strings o usa .ToString() si son números
                _spriteBatch.DrawString(FuenteJuego, "Oro: " + partidaActual.datosJugador.Oro.ToString(), new Vector2(100, 200), Color.Yellow);
                _spriteBatch.DrawString(FuenteJuego, "Reputacion: " + partidaActual.datosJugador.Reputacion.ToString(), new Vector2(100, 270), Color.White);
                _spriteBatch.DrawString(FuenteJuego, "Turno: " + partidaActual.Turno.ToString(), new Vector2(100, 340), Color.Red);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}


//fin game1

//clase partida
namespace Project1
{
    internal class Partida
    {
        public int Turno { get; set; }
        public string Nombre { get; set; }
        public Jugador datosJugador { get; set; }

        public DateTime Fecha { get; set; }

        public Partida(string Nombre, int Turno, Jugador datosJugador)
        {
            this.Nombre = Nombre;
            this.Turno = Turno;
            this.datosJugador = datosJugador;
            Fecha = DateTime.Now;
        }
    }
}

//clase jugador
namespace Project1
{
    internal class Jugador
    {
        public string Nombre { get; set; }

        public int Oro { get; set; }

        public int Reputacion { get; set; }

        public int Ejercito { get; set; }

        public Jugador(string Nombre, int Oro, int Reputacion, int Ejercito)
        {
            this.Nombre = Nombre;
            this.Oro = Oro;
            this.Ejercito = Ejercito;
        }
    }
}


namespace Project1
{
    internal class Sprite
    {
        public Texture2D texture;

        public Vector2 position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}