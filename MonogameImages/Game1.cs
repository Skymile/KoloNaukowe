using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonogameImages
{
    public static class Algorithm
    {
        public static void Initialize(GraphicsDevice device) =>
            _device = device;

        public static Texture2D Apply(Texture2D tex)
        {
            var output = new Texture2D(_device, tex.Width, tex.Height);
            var ptr = new byte[tex.Width * tex.Height * 4];
            tex.GetData(ptr);

            const int bpp = 4;
            int stride = tex.Width * bpp;
            int length = stride * tex.Height;
            int border = stride + bpp;

            ptr = Binarize(ptr);

            int P2 = -stride;
            int P8 = -bpp   ;
            int P4 = +bpp   ;
            int P6 = +stride;

            int[] bOffsets =
            {
                - stride,
                - stride + bpp,
                + bpp,
                stride + bpp,
                stride,
                stride - bpp,
                - bpp,
                - stride - bpp,
                - stride,
            };

            int Transitions(int offset)
            {
                int transitions = 0;
                for (int k = 1; k < bOffsets.Length; k++)
                    if ((
                        ptr[offset + bOffsets[k - 1]] == White &&
                        ptr[offset + bOffsets[k + 0]] == Black) 
                        )
                        //(ptr[offset + bOffsets[k - 1]] == Black &&
                        // ptr[offset + bOffsets[k + 0]] == White))
                        ++transitions;
                return transitions;
            }

            int BlackCount(int offset)
            {
                int blacks = 0;
                for (int k = 0; k < bOffsets.Length - 1; k++)
                    blacks += ptr[offset + bOffsets[k]] == Black ? 0 : 1;
                return blacks;
            }

            bool anyChanges;
            do
            {
                anyChanges = false;
                var list = new List<int>();
                for (int i = border; i < length - border; i += bpp)
                    if (ptr[i] == Black)
                    {
                        int blacks = BlackCount(i);
                        int transitions = Transitions(i);

                        if (
                            2 <= blacks && blacks <= 6 && transitions == 1 &&
                            ptr[i + P2] * ptr[i + P4] * ptr[i + P6] == 0 &&
                            ptr[i + P4] * ptr[i + P6] * ptr[i + P8] == 0)
                        {
                            list.Add(i);
                            anyChanges = true;
                        }
                    }

                foreach (var i in list)
                    ptr[i + 0] = 
                    ptr[i + 1] = 
                    ptr[i + 2] = White;
                list.Clear();

                for (int i = border; i < length - border; i += bpp)
                    if (ptr[i] == Black)
                    {
                        int blacks = BlackCount(i);
                        int transitions = Transitions(i);
                
                        if (2 <= blacks && blacks <= 6 && transitions == 1 &&
                            ptr[i + P2] * ptr[i + P4] * ptr[i + P8] == 0 &&
                            ptr[i + P2] * ptr[i + P6] * ptr[i + P8] == 0)
                        {
                            list.Add(i);
                            anyChanges = true;
                        }
                    }
                
                foreach (var i in list)
                    ptr[i + 0] =
                    ptr[i + 1] =
                    ptr[i + 2] = White;
            } while (anyChanges);

            output.SetData(ptr);
            return output;
        }

        private const byte Black = 0;
        private const byte White = 255;

        private static byte[] Binarize(byte[] arr)
        {
            for (int i = 0; i < arr.Length; i += 4)
            {
                arr[i + 0] =
                arr[i + 1] =
                arr[i + 2] = (
                    arr[i + 0] + arr[i + 1] + arr[i + 2]
                ) / 3 > 128 ? byte.MaxValue : byte.MinValue;
            }
            return arr;
        }

        private static GraphicsDevice _device;
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _input;
        private Texture2D _output;
        private Vector2 _vector;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization logic here
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Algorithm.Initialize(GraphicsDevice);

            _input = Content.Load<Texture2D>("finger 2");
            _output = Algorithm.Apply(_input);
            _vector = new Vector2(_input.Width + 20, 0);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_input, _input.Bounds, Color.White);
            _spriteBatch.Draw(_output, _vector, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
