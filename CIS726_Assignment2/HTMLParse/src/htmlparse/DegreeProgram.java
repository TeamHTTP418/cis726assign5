/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package htmlparse;

/**
 *
 * @author russfeld
 */
public class DegreeProgram {
    
    public DegreeProgram(int aid, String aname, String adescription){
        id = aid;
        name = aname;
        description = adescription;
    }
    
    int id;
    public int getId(){
        return id;
    }
    
    String name;
    public String getName(){
        return name;
    }
    
    String description;
    public String getDescription(){
        return description;
    }
    
}
