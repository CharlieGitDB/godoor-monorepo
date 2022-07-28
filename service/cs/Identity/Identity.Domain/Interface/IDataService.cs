using System;
namespace Identity.Domain.Interface
{
  public interface IDataService
  {
    T[] GetAll<T>();
    T Get<T>(string id);
    void Save<T>(T item);
    void Delete(string id);
  }
}

