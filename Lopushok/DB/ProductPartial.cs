using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lopushok.DB
{
    public partial class Product
    {
        public string Materials
        {
            get
            {
                return string.Join(", ", ProductMaterial.Select(p => p.Material.Name));
            }
            set { }
        }
    }
}
