/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package htmlparse;

import java.util.Iterator;
import java.util.LinkedList;

/**
 *
 * @author russfeld
 */
public class Course {
    public int id;
    public int getId(){ return id; }
    
    public Course(){
        requisites = "";
    }

    @Override
    public boolean equals(Object o) {
        if(o.getClass() == this.getClass()){
            Course c = (Course)o;
            if(c.prefix.equals(prefix) && c.number == number){
                return true;
            }
        }
        return false;
    }
    
    public boolean ugrad;
    public boolean getUgrad(){ return ugrad; }
    
    public boolean grad;
    public boolean getGrad(){ return grad; }
    
    public String prefix;
    public String getPrefix(){ return prefix; }
    
    public int number;
    public int getNumber(){ return number; }
    
    public void setNumber(String number){
        if(number.length() > 3){
            System.out.println("Number is too long! Truncating");
            number = number.substring(0, 3);
        }
        this.number = Integer.parseInt(number);
    }
    
    public String title;
    public String getTitle(){ return title; }
    
    public String description;
    public String getDescription(){ return description; }
    
    public int minHours;
    public int getMinHours(){ return minHours; }
    
    public int maxHours;
    public int getMaxHours(){ return maxHours; }
    
    public boolean variableHours;
    public boolean getVariableHours(){ return variableHours; }
    
    public void setHours(String input){
        if(input.contains("-")){
            if(input.contains("Variable")){
                input = input.replace("Variable", "");
            }
            if(input.contains("Var.")){
                input = input.replace("Var.", "");
            }
            if(input.contains(".")){
                input = input.replace(".", "");
            }
            if(input.contains(", Credit/No Credit only")){
                input = input.replace(", Credit/No Credit only", "");
            }
            if(input.contains(", C/NC")){
                input = input.replace(", C/NC", "");
            }
            String[] hours = input.split("-");
            minHours = Integer.parseInt(hours[0].trim());
            maxHours = Integer.parseInt(hours[1].trim());
        }else if(input.startsWith("V")){
            if(input.contains("-")){
                input = input.replace("Variable", "").trim();
                String[] hours = input.split("-");
                minHours = Integer.parseInt(hours[0]);
                maxHours = Integer.parseInt(hours[1]);
            }else{
                minHours = 0;
                maxHours = 18;
                variableHours = true;
            }
        }else if(input.contains("or")){
            String[] hours = input.split(" or ");
            minHours = Integer.parseInt(hours[0]);
            maxHours = Integer.parseInt(hours[1]);
        }else{
            if(input.contains(", C/NC")){
                input = input.replace(", C/NC", "");
            }
            try{
                minHours = Integer.parseInt(input);
                maxHours = minHours;
            }catch(Exception ex){
                if(input.equals("I")){
                    minHours = 1;
                    maxHours = 1;
                }else{
                    System.out.println("Problems parsing input: " + input);
                    minHours = 0;
                    maxHours = 18;
                    variableHours = true;
                }
            }
            
        }
    }
    
    public String requisites;
    public String getRequisites(){ return requisites; }
    
    public String semesters;
    public String getSemesters(){ return semesters; }
    
    public boolean uge;
    public boolean getUge(){ return uge; }
    
    public String kstate8;
    public String getKstate8(){ return kstate8; }
    
    public String notes;
    public String getNotes(){ return notes; }
    
    public String crosslisted;
    public String getCrosslisted(){ return crosslisted; }
    
    public boolean verify(){
        if(prefix == null || prefix.length() > 5){
            System.out.println("Prefix is missing or too long!");
            return false;
        }
        if(title == null || title.length() == 0){
            System.out.println("Title is missing!");
            return false;
        }
        if(description == null || description.length() == 0){
            System.out.println("Description is missing!");
            return false;
        }
        return true;
    }
    
    
    public String catNum(){
        if(number < 10){
            return prefix + " 00" + number;
        }else if(number < 100){
            return prefix + " 0" + number;
        }
        return prefix + " " + number;
    }
}
