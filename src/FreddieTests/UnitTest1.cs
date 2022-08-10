using System;
using FreddieShiz;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FreddieTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
		var report = Report;
		report = Corrections.RemoveLOANIDENTIFIERasArray(report);
		Console.WriteLine(report);
    }

    [Test]
	public void Test2()
    {
		var report = Report;
		var jobj = JObject.Parse(report);
		Func<JObject, JToken, string> func = (r, t) =>
		{
			JToken token = t["VERIFICATION_OF_ASSET"];
			if (token != null && token.Type == JTokenType.Array)
			{
				var rString = JsonConvert.SerializeObject(r);
				var objString = JsonConvert.SerializeObject((JObject)token[0]);
				var tokenString = JsonConvert.SerializeObject(token);
				var start = rString.IndexOf(tokenString);
				rString = rString.Remove(start, tokenString.Length);
				rString = rString.Insert(start, objString);

				Console.WriteLine(rString);
			}
			return null;
		};

		var results = Find(jobj, jobj, func);
		Console.WriteLine(results);

    }

	public string Find(JObject report, JToken token, Func<JObject, JToken, string> func)
	{
		if (token.Type == JTokenType.Object)
		{
			var r = func(report, token);
			if (r != null)
			{
				return r;
			}
			foreach (JProperty child in token.Children<JProperty>())
			{
				Find(report, child.Value, func);
			}
		}
		return null;
	}


	public static string Report = @"{
	""SchemaVersion"": ""2.4"",
	""DEAL"": {
		""LOANS"": {
			""LOAN"": {
				""LoanRoleType"": ""SubjectLoan"",
				""LOAN_IDENTIFIERS"": {
					""LOAN_IDENTIFIER"": {
						""LoanIdentifier"": ""221807000112"",
						""LoanIdentifierType"": ""LenderLoan""
					}
				}
			}
		},
		""PARTIES"": {
			""PARTY"": [
				{
					""INDIVIDUAL"": {
						""NAME"": {
							""FirstName"": null,
							""MiddleName"": null,
							""LastName"": ""DVOE Test""
						}
					},
					""ROLES"": {
						""ROLE"": {
							""ROLE_DETAIL"": {
								""PartyRoleType"": ""Borrower""
							}
						}
					},
					""TAXPAYER_IDENTIFIERS"": {
						""TAXPAYER_IDENTIFIER"": {
							""TaxpayerIdentifierType"": ""SocialSecurityNumber"",
							""TaxpayerIdentifierValue"": ""xxx-xx-1234""
						}
					}
				}
			]
		},
		""SERVICES"": {
			""SERVICE"": {
				""VERIFICATION_OF_ASSET"": [
					{
						""REPORTING_INFORMATION"": {
							""ReportIdentifierType"": ""ReportID"",
							""ReportingInformationIdentifier"": ""d1f0c4f4-d1a0-4ba9-bd2d-d05580c85935"",
							""ReportDateTime"": ""2022-05-18""
						},
						""SERVICE_PRODUCT_FULFILLMENT"": {
							""SERVICE_PRODUCT_FULFILLMENT_DETAIL"": {
								""ServiceProductFulfillmentIdentifier"": ""VOETRANSACTIONS"",
								""VendorOrderIdentifier"": ""FF_d1f0c4f4-d1a0-4ba9-bd2d-d05580c85935_Asset""
							}
						},
						""VERIFICATION_OF_ASSET_RESPONSE"": {
							""ASSETS"": {
								""ASSET"": []
							}
						}
					}
				],
				""STATUSES"": {
					""STATUS"": {
						""StatusCode"": ""success"",
						""StatusDescription"": null
					}
				}
			}
		}
	}
}";
}