using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    class DcmAppQueue
    {
        private object syncRoot = new object();

        public class Entity
        {
            public int CanId { get; set; }
            public List<byte> Data { get; set; }
        }

        private Queue<Entity> entities = new Queue<Entity>();

        public void Enqueue(int canId, List<byte> data)
        {
            Enqueue(new Entity
            {
                CanId = canId,
                Data = data
            });
        }

        public void Enqueue(Entity entity)
        {
            lock(syncRoot)
            {
                entities.Enqueue(entity);
            }
        }

        public Entity Dequeue()
        {
            lock(syncRoot)
            {
                if (0 == entities.Count)
                {
                    return null;
                }
                return entities.Dequeue();
            }
        }
    }
}
