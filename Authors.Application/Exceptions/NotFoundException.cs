namespace Authors.Application.Exceptions
{
    public  class NotFoundException: Exception
    {

        public NotFoundException(string mesage): base(mesage) { }
    }
}
