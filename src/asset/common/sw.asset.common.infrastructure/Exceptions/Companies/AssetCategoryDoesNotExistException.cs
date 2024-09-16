using System;

namespace sw.asset.common.infrastructure.Exceptions.Companies;

public class AssetCategoryDoesNotExistException : Exception {
  public long AssetCategoryId { get; }

  public AssetCategoryDoesNotExistException(long AssetCategoryId) {
    this.AssetCategoryId = AssetCategoryId;
  }

  public override string Message => $"AssetCategory with Id: {this.AssetCategoryId}  doesn't exists!";
}