using InciportWebService.Application;
using InciportWebService.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Data.Persistence.Testdata {

  public class WorkerTeamsTestDataFactory {
    private readonly ApplicationDbContext _dbContext;

    public WorkerTeamsTestDataFactory(ApplicationDbContext dbContext) {
      _dbContext = dbContext;
    }

    public WorkerTeamsTestData Create(int minCount, int maxCount, MunicipalityEntity trackedMunicipality) {
      WorkerTeamsTestData data = new WorkerTeamsTestData(minCount, maxCount);
      trackedMunicipality.WorkerTeams.AddRange(data.WorkerTeams);
      _dbContext.SaveChanges();
      return data;
    }
  }

  public class WorkerTeamsTestData {
    public List<WorkerTeam> WorkerTeams { get; private set; }
    private Random _random;
    private int _counter;

    public WorkerTeamsTestData(int minCount, int maxCount) {
      WorkerTeams = new List<WorkerTeam>();
      _random = new Random();
      Seed(minCount, maxCount);
    }

    public WorkerTeam ChooseNext() {
      return WorkerTeams[_counter++];
    }

    public WorkerTeam ChooseRandom() {
      return WorkerTeams[_random.Next(0, WorkerTeams.Count)];
    }

    public void Seed(int minCount, int maxCount) {
      int count = _random.Next(minCount, maxCount + 1);

      for (int i = 0; i < count; i++) {
        char letter = (char)(i + 65);
        WorkerTeams.Add(new WorkerTeam("Team " + letter));
      }
    }
  }
}