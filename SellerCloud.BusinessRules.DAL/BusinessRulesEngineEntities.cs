namespace SellerCloud.BusinessRules.DAL
{
    public partial class BusinessRulesEngineEntities : IBusinessRulesEngineContext
    {
        public BusinessRulesEngineEntities(string connectionString)
            : base(connectionString)
        {
            System.Diagnostics.Debug.WriteLine($"BusinessRulesEngineEntities: {this.GetHashCode()}");

            System.Diagnostics.Debug.WriteLine($"BusinessRulesEngineEntities-object: {((object)this).GetHashCode()}");
        }
    }

}
