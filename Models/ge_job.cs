using System;
using System.Threading;

// https://stackoverflow.com/questions/1387103/best-way-to-decouple-for-parallel-processing-a-web-applications-non-immediate
// ThreadPool in .NET is queue based worker pool, however its used internally by ASP.NET host process, so if you try to utilize ThreadPool more, you may reduce performance of Web Server.

// So you must create your own thread, mark it as background and let it poll every few seconds for job availability.

// The best way to do is, create a Job Table in database as follow,

// Table: JobQueue
// JobID (bigint, auto number)
// JobType (sendemail,calcstats)
// JobParams (text)
// IsRunning (true/false)
// IsOver (true/false)
// LastError (text)
// JobThread class could be like following.
// class ge_job {
//     public Guid Id {get;set;}

    




// }
// class JobThread{
//     static Thread bgThread = null;
//     static AutoResetEvent arWait = new AutoResetEvent(false);

//     public static void ProcessQueue(Job job)
//     {
//          // insert job in database
//          job.InsertInDB();

//          // start queue if its not created or if its in wait
//          if(bgThread==null){
//               bgThread = new Thread(new ..(WorkerProcess));
//               bgThread.IsBackground = true;
//               bgThread.Start();
//          }
//          else{
//               arWait.Set();
//          }
//     }

//     private static void WorkerProcess(object state){
//          while(true){
//               Job job = GetAvailableJob( 
//                         IsProcessing = false and IsOver = flase);
//               if(job == null){
//                    arWait.WaitOne(10*1000);// wait ten seconds.
//                                            // to increase performance
//                                            // increase wait time
//                    continue;
//               }
//               job.IsRunning = true;
//               job.UpdateDB();
//               try{

//               //
//               //depending upon job type do something...
//               }
//               catch(Exception ex){
//                    job.LastError = ex.ToString(); // important step
//                    // this will update your error in JobTable
//                    // for later investigation
//                    job.UpdateDB();
//               }
//               job.IsRunning = false;
//               job.IsOver = true;
//               job.UpdateDB();
//          }
//     }
// }