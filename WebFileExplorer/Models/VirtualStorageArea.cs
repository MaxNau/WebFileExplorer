
namespace WebFileExplorer.Models
{
    public abstract class VirtualStorageArea
    {
        protected string name;
        protected string path;
        protected string type;

        public VirtualStorageArea(string name, string path, string type = "Virtual Storage Area")
        {
            this.name = name;
            this.path = path;
            this.type = type;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    public class Volume : VirtualStorageArea
    {
        public Volume(string name, string path, string type = "Volume") : base (name, path, type)
        {

        }
    }

    public class Folder : VirtualStorageArea
    {
        public Folder(string name, string path, string type = "Folder") : base(name, path, type)
        {
        }
    }

    public class File : VirtualStorageArea
    {
        private long size;
        public string FormatedSize { get;set; }

        public File(string name, string path, string type = "File") : base(name, path, type)
        {
        }

        public long Size
        {
            get
            {
                return size;
            }
            set
            {
                FormatedSize = FormatSize(value);
                size = value;
            }
        }

        // This should be in ViewModel
        // Format size for better thouthands recognition
        private string FormatSize(long size)
        {
            var nfi = (System.Globalization.NumberFormatInfo)System.Globalization.CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            return size.ToString();
        }
    }
}