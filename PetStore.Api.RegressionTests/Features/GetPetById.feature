Feature: Get one pet by Id
The following test scenarios test the GET one pet by ID endpoint

Scenario: Verify the data for one pet is received when a request is made through Get pet by ID endpoint
	Given I am pointed to the PetStore GET Pet By ID endpoint
	And I am provided with a Pet ID 2241 to use
	When I make a GET request to the endpoint
	Then I should receive a successful status code 200 for the GET request
	And I should see a valid response for one pet as below
	| Id	| CategoryId	| CategoryName	| Name	| PhotoUrls	| TagsId	| TagsName			| Status	|
	| 2241	| 224101        | Cats			| Cello | NA		| 22410101	| Cello-22410101	| available	|

Scenario: Verify the valid error message is pet is not found for the requested ID
	Given I am pointed to the PetStore GET Pet By ID endpoint
	And I am provided with a Pet ID 2249 to use
	When I make a GET request to the endpoint
	Then I should receive a error status code 404 for the GET request
	And I should see the error message as below
	| Code | Type    | Message		 | 
	| 1    | error   | Pet not found |