Overview: 

1. Considered Json input as database and implemented solution using .net core 3.1 
2. Implemented layered architechure and dependency injection(.net core inbuilt)
3. Swagger configuration added to solution to run and verify the solution
4. Implemented 2 endpoints 2 calculate avg usage of tablet and avg usage of tablet for acadamy
5. Considered below scenarios for Avg calculation:
    Scenario 1:    
      Initial reading : 50 (2019-05-17T09:00:00)
      Next Readings : 
      60(2019-05-17T10:00:00)
      70(2019-05-17T11:00:00)
      90(2019-05-17T12:00:00)
    Usage: 0 (as continuously chargig got incresed)
    
    
    Scenario 2:
      Initial reading : 50 (2019-05-17T09:00:00)
      Next Readings : 
      60(2019-05-17T10:00:00)
      70(2019-05-17T11:00:00)
      30(2019-05-17T12:00:00)
    Usage calculated by ignoring 60,70 and for this daily avg calculated by considering time of initial reading(2019-05-17T09:00:00)
     
    
    Scenario 3:
      Initial reading : 100 (2019-05-17T09:00:00)
      Next Readings : 
      90(2019-05-17T10:00:00)
      80(2019-05-17T11:00:00)
      100(2019-05-17T12:00:00)
    Usage calculated by ignoring 100 and for this daily avg calculated by considering endtime of 80 reading noted


Further Extension for the Project:

1. Identifying which Acadamy is having more battery issues by processing available data
2. Sending notifications to the respective person after receiving the data points and calculating avg usage of battery if it requires maintenance (by implementing event-driven system)
