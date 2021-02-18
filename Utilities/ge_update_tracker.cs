using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ge_repository.OtherDatabase  {

public class ge_update_tracker<TEntity> where TEntity:class {

public List<ge_update<TEntity>> updates {get;set;} =  new List<ge_update<TEntity>>();

}

public class ge_update<TEntity> where TEntity:class {
public string userId {get;set;}
public DateTime updateDT {get;set;}
public TEntity row {get;set;}
public ge_update (string UserId, DateTime UpdateDT, TEntity Row) {

        userId = UserId;
        updateDT = UpdateDT;
        row = Row;

}

    
}





}
