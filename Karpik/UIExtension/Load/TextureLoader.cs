namespace Karpik.UIExtension.Load
{
    public class TextureLoader
    {
        public ITextureLoader Loader { get; set; } = new ResourcesLoader();
        
        public static TextureLoader Instance { get; } = new TextureLoader();
        
        private TextureLoader() {}

        public TextureInfo this[string key] => Load(key);
        public TextureInfo Load(string key) => Loader.Load(key);
    }
}