Feature: Get all pets by status
The following test scenarios test the GET all pets by status endpoint

Scenario: Verify the data for all pets is received when a request is made through Get pets by status endpoint
	Given I am pointed to the PetStore GET Pet By status endpoint
	And I am provided with a Pet status sold
	When I make a GET request to the endpoint
	Then I should receive a successful status code 200 for the GET request
	And I should see a valid response for multiple pets below
	| Id	| CategoryId	| CategoryName	| Name		| PhotoUrls	| TagsId	| TagsName			| Status	|
	| 2242	| 224201        | Dogs			| Striker	| NA		| 22420101	| Striker-22420101	| sold		|
	| 2243	| 224301        | Birds			| Snow		| NA		| 22430101	| Snow-22430101		| sold		|
	| 2244	| 224401        | Rabbits		| Misty		| NA		| 22440101	| Misty-22440101	| sold		|
	| 2245	| 224501        | Cats			| Aries     | NA		| 22450101	| Aries-22450101	| sold		|