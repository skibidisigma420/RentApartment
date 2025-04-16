namespace ClassLibrary1
{
    public class FilePath
    {
        public string Path { get; }

        public FilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be empty.");

            Path = path;
        }

        public override bool Equals(object? obj)
        {
            return obj is FilePath other && Path == other.Path;
        }

        public override int GetHashCode() => Path.GetHashCode();
    }
}
