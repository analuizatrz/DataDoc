namespace DataDoc.Domain
{
	public interface IRepository<T>
	{
		Database Read(string name);
		void Save(T entity);
	}
}
