Feature: Post new pet to the store
The following test scenarios test the POST add new pet endpoint

Scenario: Verify the data for one pet is registered when a request is made through POST add new pet endpoint
	Given I am pointed to the PetStore POST add new pet endpoint
	And I am provided with the pet data as below
	| Id	| CategoryId	| CategoryName	| Name	| PhotoUrls	| TagsId	| TagsName			| Status		|
	| 2240	| 2240          | Dogs			| Dolly | NA		| 22400101	| Dolly-22400101	| available		|
	When I make a POST request to the endpoint
	Then I should receive a successful status code 200 for the POST request
	And I should see a valid response for one pet as below
	| Id	| CategoryId	| CategoryName	| Name	 | PhotoUrls	| TagsId	| TagsName		| Status		|
	| 2240	| 2240          | Dogs			| Dolly | NA		| 22400101	| Dolly-22400101	| available		|