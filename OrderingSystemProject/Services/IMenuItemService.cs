using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IMenuItemService
{
    List<MenuItem> GetAll();
    MenuItem? GetById(int id);
    void Add(MenuItem item);
    void Update(MenuItem item);
    void Delete(MenuItem item);
    void Activate(MenuItem item);
    void Deactivate(MenuItem item);
}