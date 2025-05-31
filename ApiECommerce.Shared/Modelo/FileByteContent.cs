using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiECommerce.Modelo
{
    public class FileByteContent
    {
        public byte[] data { get; set; }
        public string type { get; set; }
        public string name { get; set; }

        public FileByteContent(byte[] data, string type, string name)
        {
            this.data = data;
            this.type = type;
            this.name = name;
        }
    }
}