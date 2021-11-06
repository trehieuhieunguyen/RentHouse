namespace RentHouse.Reponsitory.IReponsitory
{
    public interface IDashBoardReponsitory
    {
        Task<int> GetCountUserOneMonth(int i);
    }
}
