namespace ImageManager.NHibernate.Interfaces
{
	public interface IIdentityEntity<TKey>
	{
		TKey Id { get; }
	}
}
