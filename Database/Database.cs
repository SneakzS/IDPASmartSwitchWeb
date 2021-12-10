using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSwitchWeb.Database
{
    public class Database<T> : IDisposable where T : class
    {
        private bool disposed = false;
        private DatabaseContext context = null;
        
        protected DbSet<T> DbSet
        {
            get;set;
        }

		public Database()
		{
			context = new DatabaseContext();
			DbSet = context.Set<T>();
		}

		public Database(DatabaseContext context)
		{
			this.context = context;
		}
		public List<T> GetAll()
		{
			return DbSet.ToList();
		}

		public T Get(int id)
		{
			return DbSet.Find(id);
		}

		public virtual void Add(T entity)
		{
			DbSet.Add(entity);
			context.Entry<T>(entity).State = EntityState.Added;
		}

		public virtual void Update(T entity)
		{
			context.Entry<T>(entity).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			DbSet.Remove(DbSet.Find(id));
			context.Entry<T>(DbSet.Find(id)).State = EntityState.Deleted;
		}

		public void SaveChanges()
		{
			context.SaveChanges();
		}
		public void Dispose()
        {
            if (!disposed)
            {
                context.Dispose();
                disposed = true;
            }
        }
    }
}
