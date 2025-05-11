using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCinema.common.entities
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
