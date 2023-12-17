namespace Database.Data.Entities
{
    public enum ApiType : byte
    {

        GetForGrid = 1,
        GetById = 2,
        GetList = 3,
        Create = 4,
        Edit = 5,
        DeleteById = 6,
        DeleteByIds = 7,
        Active = 8,
        Deactive = 9,

        Get = 10,
        Post = 11,
        Patch = 12,

    }


}
