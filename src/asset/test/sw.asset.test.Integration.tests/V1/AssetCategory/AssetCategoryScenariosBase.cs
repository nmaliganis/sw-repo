using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.Integration.tests.V1.AssetCategory
{
    public class AssetCategoryScenariosBase : RegisterUserScenariosBase
    {
        private const string AssetCategoryApiUrlBase = "/api/v1/AssetCategories";

        public static class Get
        {
            public static string GetAssetCategoryByIdAsync(long id)
            {
                return $"{AssetCategoryApiUrlBase}/{id}";
            }

            public static string GetAssetCategoriesAsync()
            {
                return $"{AssetCategoryApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostAssetCategoryAsync()
            {
                return $"{AssetCategoryApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateAssetCategoryAsync(long id)
            {
                return $"{AssetCategoryApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftAssetCategoryAsync(long id)
            {
                return $"{AssetCategoryApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardAssetCategoryAsync(long id)
            {
                return $"{AssetCategoryApiUrlBase}/hard/{id}";
            }
        }
    }
}
