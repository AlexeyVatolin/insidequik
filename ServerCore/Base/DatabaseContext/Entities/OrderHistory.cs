using ServerCore.Base.DatabaseContext.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Inside.Core.Base.DatabaseContext.Entities
{
    public class OrderHistory 
    {
        //todo add IOrder and IContract
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long OrderId { set; get; }
        public string UserId { set; get; }
        public User User { set; get; }

    }
}
