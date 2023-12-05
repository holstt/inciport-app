from locust import HttpUser, task, between




# A user class represents one user (or a swarming locust if you will). Locust will spawn one instance of the User class for each user that is being simulated. 
class AppUser(HttpUser):
    # host = 'http://api-dev.inciport.rocks'
    # host = 'http://api-cicd-webservice-t1.inciport.rocks'
    host = 'https://inciport.rocks'
    # host = 'https://localhost:5000/test'

    # When a task has finished executing, the User will then sleep during its wait time. After its wait time it’ll pick a new task and keep repeating that.
    # wait_time = between(1,1)

    # Tasks are picked at random, but you can give them different weighting.
    # @task(3)

    @task
    def get(self):
        self.client.get("/api/municipalities/5/inciports"), 

    # @task
    # def submit_incident_report(self):
    #     self.client.post("/api/municipalities/5/inciports", 
    #         json={
    #         "chosenMainCategory": {
    #             "id": 10,
    #             "chosenSubCategory": {
    #             "id":11
    #             }
    #         },
    #         "location": {
    #             "address": {
    #             "street": "string",
    #             "city": "string",
    #             "zipCode": "stng",
    #             "country": "string",
    #             "municipality": "string"
    #             },
    #             "longitude": 0,
    #             "latitude": 0
    #         },
    #         "description": "string",
    #         "contactInformation": {
    #             "name": "string",
    #             "phoneNumber": "string",
    #             "email": "string"
    #         }
    #         }
    #         )


# @task
# def submit_incident_report(self):
#     self.client.post("/api/municipalities/2/inciports", 
#         json={
#         "chosenMainCategory": {
#             "id": 6,
#             "chosenSubCategory": {
#             "id": 7
#             }
#         },
#         "location": {
#             "address": {
#             "street": "string",
#             "city": "string",
#             "zipCode": "string",
#             "country": "string",
#             "municipality": "string"
#             },
#             "longitude": 0,
#             "latitude": 0
#         },
#         "description": "string",
#         "contactInformation": {
#             "name": "string",
#             "phoneNumber": "string",
#             "email": "string"
#         }
#         }
#         )