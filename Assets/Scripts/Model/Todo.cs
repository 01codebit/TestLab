using System;

namespace TestLab.EventChannel.Model
{
    [Serializable]
    public class Todo : IDataModel
    {
        public int userId;
        public int id;
        public string title;
        public bool completed;

        public new string ToString()
        {
            return $"Todo [UserId:{userId}, Id:{id}, Title:{title}, Completed:{completed}]";
        }

        public string GetField(string field)
        {
            return field switch
            {
                "userId" => $"{userId}",
                "id" => $"{id}",
                "title" => title,
                "completed" => $"{completed}",
                _ => null
            };
        }
    }
}