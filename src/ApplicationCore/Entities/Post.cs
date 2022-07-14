using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities;

public class Post : BaseEntity, IAggregateRoot
{
    public Post(string title, string content, int userId)
    {
        Title = title;
        Content = content;
        UserId = userId;
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime DateCreated { get; private set; } = DateTime.Now;
    public int UserId { get; private set; }
}