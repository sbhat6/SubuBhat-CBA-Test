Feature: Post an image to one pet
The following test scenarios test the POST add new image to pet endpoint

Scenario Outline: Verify the image for one pet is registered when a request is made through POST add new image to pet endpoint
	Given I am pointed to the PetStore POST add new image endpoint
	And I am provided with a Pet ID <PetId> and the image filename <FileName> and image file size <FileSize>
	When I make a POST request to the endpoint
	Then I should receive a successful status code 200 for the POST request
	And I should see a valid response for file upload as below
	| Code | Type    | Message                                                         | 
	| 200  | unknown | additionalMetadata: null\nFile uploaded to filename, size bytes |
	Examples: 
	| PetId | FileName            | FileSize |
	| 2242  | Pet_2242_Dog.jpg    | 5558     |
	| 2243  | Pet_2243_Bird.jpg   | 5105     |
	| 2244  | Pet_2244_Rabbit.jpg | 15516    |
	| 2245  | Pet_2245_Cat.jpg    | 3595     |