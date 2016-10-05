using Crondale.AzureWrapper.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holmlia.Core
{
    public class Player : EntityModel
    {
        public String Id
        {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }

        public String Name {get;set;}


        public Player Save()
        {
            TableHelper.Save<Player>(this);

            return this;
        }

        public static Player Create(string name)
        {

            Player player = new Player();
            player.PartitionKey = "1";
            player.Id = Guid.NewGuid().ToString();
            player.Name = name;
            return player;
        }

        public static IEnumerable<Player> GetAll()
        {
            return TableHelper.GetAll<Player>();
        }




    }
}
