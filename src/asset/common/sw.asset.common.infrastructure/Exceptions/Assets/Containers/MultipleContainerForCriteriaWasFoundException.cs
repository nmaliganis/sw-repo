using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class MultipleContainerForCriteriaWasFoundException : Exception
{
	public string ExcMessage { get; private set; }

	public MultipleContainerForCriteriaWasFoundException(string excMessage)
	{
		this.ExcMessage = excMessage;
	}

	public override string Message => $" Criteria - Container for: {ExcMessage}";
}