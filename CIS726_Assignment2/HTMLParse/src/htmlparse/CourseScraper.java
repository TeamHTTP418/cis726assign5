/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package htmlparse;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.StaxDriver;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Iterator;
import java.util.LinkedList;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

/**
 *
 * @author russfeld
 */
public class CourseScraper { 
    
    static LinkedList<Course> courselist;
    static LinkedList<DegreeProgram> degreePrograms;
    static LinkedList<ElectiveList> electiveLists;
    static LinkedList<ElectiveListCourse> electiveListCourses;
    static LinkedList<RequiredCourse> requiredCourses;
    static LinkedList<ElectiveCourse> electiveCourses;
    static LinkedList<PrerequisiteCourse> prerequisiteCourses;
    static int count;
    
    public static void main(String[] args){
        courselist = new LinkedList<Course>();
        degreePrograms = new LinkedList<DegreeProgram>();
        electiveLists = new LinkedList<ElectiveList>();
        electiveListCourses = new LinkedList<ElectiveListCourse>();
        requiredCourses = new LinkedList<RequiredCourse>();
        electiveCourses = new LinkedList<ElectiveCourse>();
        prerequisiteCourses = new LinkedList<PrerequisiteCourse>();
        count = 1;
        scrapeUG();
        scrapeGrad();
        createElectiveLists();
        createDegreePrograms();
        dealWithPrerequisites();
        
        //Output Courses
        XStream xstream = new XStream(new StaxDriver());
        BufferedWriter out = null;
        try{
            out = new BufferedWriter(new FileWriter("ksu_courses.xml"));
            out.write("<courses>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!courselist.isEmpty()){
            Course course = courselist.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</courses>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Degree Programs
        try{
            out = new BufferedWriter(new FileWriter("ksu_degreeprograms.xml"));
            out.write("<degreeprograms>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!degreePrograms.isEmpty()){
            DegreeProgram course = degreePrograms.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</degreeprograms>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Elective Lists
        try{
            out = new BufferedWriter(new FileWriter("ksu_electivelists.xml"));
            out.write("<electivelists>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!electiveLists.isEmpty()){
            ElectiveList course = electiveLists.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</electivelists>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Elective List Courses
        try{
            out = new BufferedWriter(new FileWriter("ksu_electivelistcourses.xml"));
            out.write("<electivelistcourses>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!electiveListCourses.isEmpty()){
            ElectiveListCourse course = electiveListCourses.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</electivelistcourses>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Required Courses
        try{
            out = new BufferedWriter(new FileWriter("ksu_requiredcourses.xml"));
            out.write("<requiredcourses>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!requiredCourses.isEmpty()){
            RequiredCourse course = requiredCourses.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</requiredcourses>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Elective Courses
        try{
            out = new BufferedWriter(new FileWriter("ksu_electivecourses.xml"));
            out.write("<electivecourses>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!electiveCourses.isEmpty()){
            ElectiveCourse course = electiveCourses.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</electivecourses>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
        
        //Output Prerequisite Courses
        try{
            out = new BufferedWriter(new FileWriter("ksu_prerequisitecourses.xml"));
            out.write("<prerequisitecourses>\n");
        }catch(Exception e){
            e.printStackTrace();
        }
        while(!prerequisiteCourses.isEmpty()){
            PrerequisiteCourse course = prerequisiteCourses.remove();
            try {
                out.write(xstream.toXML(course).substring(22) +"\n");
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        }
        try{
            out.write("</prerequisitecourses>\n");
            out.flush();
        }catch(Exception e){
            e.printStackTrace();
        }
        
        try{
            out.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }
    
    public static void dealWithPrerequisites(){
        Iterator<Course> iter = courselist.iterator();
        while(iter.hasNext()){
            Course course = iter.next();
            if(course.requisites.length() > 0){
                Iterator<Course> iter2 = courselist.iterator();
                while(iter2.hasNext()){
                    Course course2 = iter2.next();
                    if(course.requisites.contains(course2.catNum())){
                        PrerequisiteCourse prereq = new PrerequisiteCourse();
                        prereq.prerequisiteCourse = course2.id;
                        prereq.prerequisiteFor = course.id;
                        prereq.id = prerequisiteCourses.size() + 1;
                        prerequisiteCourses.add(prereq);
                    }
                }
            }
        }
    }
    
    public static void createDegreePrograms(){
        degreePrograms.add(new DegreeProgram(1, "CIS - B.S. in Computer Science (Computer Science Option)", "This is the degree program for Computer Science"));
        degreePrograms.add(new DegreeProgram(2, "CIS - B.S. in Computer Science (Software Engineering Option)", "This is the degree program for Software Engineering"));
        degreePrograms.add(new DegreeProgram(3, "CIS - B.S. in Information Systems", "This is the degree program for Information Systems"));
        degreePrograms.add(new DegreeProgram(4, "CIS - M.S. in Computer Science", "This is the masters degree program for Computer Science"));
        degreePrograms.add(new DegreeProgram(5, "CIS - M.S. in Software Engineering", "This is the masters degree program for Software Engineering"));
        degreePrograms.add(new DegreeProgram(6, "CIS - Ph.D. in Computer Science", "This is the doctoral program for Computer Science"));
        Iterator<Course> iter = courselist.iterator();
        while(iter.hasNext()){
            Course here = iter.next();
            if(here.title.contains("Salina campus")){
                continue;
            }
            if(here.prefix.contains("CIS") && here.number == 115){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 1));
            }
            if(here.prefix.contains("MATH") && here.number == 220){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 1));
            }
            if(here.prefix.contains("ENGL") && here.number == 100){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 1));
            }
            if(here.prefix.contains("COMM") && (here.number == 105 || here.number == 106)){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 1));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 2));
            }
            if(here.prefix.contains("CIS") && here.number == 200){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 2));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 2));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 3));
            }
            if(here.prefix.contains("MATH") && here.number == 221){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 2));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 2));
            }
            if(here.prefix.contains("ECE") && here.number == 241){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 2));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 2));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 2));
            }
            if(here.prefix.contains("CIS") && here.number == 300){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 4));
            }
            if(here.prefix.contains("CIS") && here.number == 301){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 4));
            }
            if(here.prefix.contains("ENGL") && here.number == 200){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 3));
            }
            if(here.prefix.contains("ECON") && here.number == 110){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 3));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 3));
            }
            if(here.prefix.contains("CIS") && here.number == 308){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 4));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 4));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 5));
            }
            if(here.prefix.contains("CIS") && here.number == 501){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 4));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 4));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 5));
            }
            if(here.prefix.contains("MATH") && here.number == 510){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 4));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 4));
            }
            if(here.prefix.contains("CIS") && here.number == 415){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 5));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 5));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 505){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 5));
            }
            if(here.prefix.contains("CIS") && here.number == 450){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 6));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 5));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 6));
            }
            if(here.prefix.contains("ENGL") && here.number == 575){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 6));
            }
            if(here.prefix.contains("ENGL") && here.number == 516){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 6));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 5));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 5));
            }
            if(here.prefix.contains("CIS") && here.number == 520){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 560){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 7));
            }
            if(here.prefix.contains("MATH") && here.number == 551){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 7));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 598){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 8));
            }
            if(here.prefix.contains("STAT") && here.number == 510){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 1, 8));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 6));
            }
            if(here.prefix.contains("CIS") && here.number == 625){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 6));
            }
            if(here.prefix.contains("CIS") && here.number == 540){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 562){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 7));
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 541){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 8));
            }
            if(here.prefix.contains("CIS") && here.number == 544){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 2, 8));
            }
            if(here.prefix.contains("MATH") && here.number == 205){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 1));
            }
            if(here.prefix.contains("CMST") && here.number == 135){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 2));
            }
            if(here.prefix.contains("ACCTG") && here.number == 231){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 5));
            }
            if(here.prefix.contains("STAT") && here.number == 325){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 5));
            }
            if(here.prefix.contains("CIS") && here.number == 526){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 6));
            }
            if(here.prefix.contains("CIS") && here.number == 525){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 543){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 7));
            }
            if(here.prefix.contains("CIS") && here.number == 597){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 3, 8));
            }
            
            //MSE
            if(here.prefix.contains("CIS") && here.number == 740){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
            if(here.prefix.contains("CIS") && here.number == 744){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
            if(here.prefix.contains("CIS") && here.number == 748){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
            if(here.prefix.contains("CIS") && here.number == 771){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
            if(here.prefix.contains("CIS") && here.number == 841){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
            if(here.prefix.contains("CIS") && here.number == 895){
                requiredCourses.add(new RequiredCourse(requiredCourses.size(), here.id, 5, 1));
            }
        }
        //electives for CS BS
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 1, 1, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 1, 3, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 1, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 1, 5, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 1, 6, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 2, 7, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 2, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 3, 2, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 3, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 3, 5, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 3, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 4, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 5, 5, 6));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 5, 6, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 5, 7, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 1, 5, 8, 3));
        
        //electives for SE BS
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 1, 1, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 1, 3, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 1, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 1, 5, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 1, 6, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 2, 7, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 2, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 3, 2, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 3, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 3, 5, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 3, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 4, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 5, 5, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 5, 6, 6));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 5, 7, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 2, 5, 8, 3));
        
        //electives for IS BS
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 1, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 2, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 3, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 6, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 7, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 1, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 2, 7, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 2, 8, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 3, 2, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 3, 4, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 3, 8, 4));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 4, 4, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 1, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 3, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 4, 2));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 5, 3));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 6, 6));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 3, 5, 8, 3));
        
        //electives for CS MS
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 6, 1, 5));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 7, 1, 5));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 8, 1, 5));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 9, 1, 5));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 10, 1, 5));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 4, 11, 1, 5));
        
        //electives for CS MS
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 5, 12, 1, 6));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 5, 13, 1, 6));
        
        //electives for CS PHD
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 6, 14, 1, 1));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 6, 15, 1, 1));
        electiveCourses.add(new ElectiveCourse(electiveCourses.size(), 6, 16, 1, 1));
    }
    
    public static void createElectiveLists(){
        electiveLists.add(new ElectiveList(1, "Engineering - Humanities and Social Science Electives", "Hum. & Soc. Sci."));
        electiveLists.add(new ElectiveList(2, "CIS - Technical Electives", "Tech Elect."));
        electiveLists.add(new ElectiveList(3, "CIS - Natural Sciences Electives", "Nat. Sci."));
        electiveLists.add(new ElectiveList(4, "CIS - Communication Electives", "Comm. Elect."));
        electiveLists.add(new ElectiveList(5, "Unrestricted Electives", "Unrest."));
        
        electiveLists.add(new ElectiveList(6, "CIS - Master of Science Implementation", "MS Impl."));
        electiveLists.add(new ElectiveList(7, "CIS - Master of Science Languages", "MS Lang."));
        electiveLists.add(new ElectiveList(8, "CIS - Master of Science Systems", "MS Sys."));
        electiveLists.add(new ElectiveList(9, "CIS - Master of Science Structures", "MS Struct."));
        electiveLists.add(new ElectiveList(10, "CIS - Master of Science Theory", "MS Theory"));
        electiveLists.add(new ElectiveList(11, "CIS - Master of Science Specialization", "MS Spec."));
        
        electiveLists.add(new ElectiveList(12, "CIS - Master of Software Engineering Specialization", "MSE Spec."));
        electiveLists.add(new ElectiveList(13, "CIS - Master of Software Engineering Technical Electives", "MSE Tech Elect."));
        
        electiveLists.add(new ElectiveList(14, "CIS - Doctoral Courses", "PhD Courses"));
        electiveLists.add(new ElectiveList(15, "CIS - Doctoral Courses 800 Level or Higher", "PhD 800+"));
        electiveLists.add(new ElectiveList(16, "CIS - Doctoral Research", "PhD Research"));
        Iterator<Course> iter = courselist.iterator();
        while(iter.hasNext()){
            Course here = iter.next();
            if(here.prefix.equals("ARCH")){
                int num = here.number;
                if(num == 240 || num == 290 || num == 301){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("AMETH")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("ANTH")){
                int num = here.number;
                if((num >= 200 && num <= 260) || (num >= 503 && num <= 517) || (num >= 524 && num <= 618) || (num >= 630 && num <= 634) || num == 673 || num == 676 || num == 685){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("ART")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("CHM")){
                int num = here.number;
                if(num == 315 || num == 650){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("COMM")){
                int num = here.number;
                if(num == 120 || num == 320 || num == 322 || num == 323 || num == 330 || num == 331 || num == 420 || num == 470 || num == 480 || (num >= 430 && num <= 435)){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("DANCE")){
                int num = here.number;
                if((num >= 120 && num <= 195) || num == 205 || num == 459){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("ECON")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("ENGL")){
                int num = here.number;
                if((num >= 220 && num <= 298) || (num >= 315 && num <= 390) || num == 420 || num == 440 || num == 445 || num == 450 || num == 470 || num == 476 || num == 490 || num == 525 || num == 545 || num == 580){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }            
            }
            if(here.prefix.equals("GEOG")){
                int num = here.number;
                if(num != 221 && num != 321 && num != 445 && num != 508 && num != 535 && num != 700 && num != 702 && num != 705 && num != 708 && num != 709 && num != 711 && num != 735 && num != 745 && num != 765 && num != 795){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }            
            }
            if(here.prefix.equals("HIST")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("LEAD")){
                int num = here.number;
                if(num == 350 || num == 450){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("MC")){
                int num = here.number;
                if(num == 531 || (num >= 110 && num <= 112) || (num >= 710 && num <= 725)){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("ARAB") || here.prefix.equals("CHINE") || here.prefix.equals("CZECH") || here.prefix.equals("FREN") || here.prefix.equals("GRMN") || here.prefix.equals("HINDI") || here.prefix.equals("ITAL") || here.prefix.equals("JAPAN") || here.prefix.equals("LATIN") || here.prefix.equals("LG") || here.prefix.equals("MLANG") || here.prefix.equals("PORT") || here.prefix.equals("RUSSN") || here.prefix.equals("URDU") || here.prefix.equals("SPAN") || here.prefix.equals("SWAH")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("MUSIC")){
                int num = here.number;
                if((num >= 103 && num <= 140) || (num >= 203 && num <= 210) || (num >= 230 && num <= 239) || (num >= 251 && num <= 260) || (num >= 280 && num <= 299) || (num >= 320 && num <= 373) || (num >= 400 && num <= 404) || (num >= 408 && num <= 417) || (num >= 427 && num <= 490) || (num >= 420 && num <= 424) || num == 100 || num == 160 || num == 170 || num == 171 || num == 225 || num == 245 || num == 250 || num == 310 || num == 385){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("PHILO")){
                int num = here.number;
                if(num != 110 && num != 320 && num != 510){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }            
            }
            if(here.prefix.equals("POLSC")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("PSYCH")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("SOCIO")){
                int num = here.number;
                if(num != 520 && num != 522 && num != 510){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }            
            }
            if(here.prefix.equals("THTRE")){
                int num = here.number;
                if(num == 361 || (num >= 211 && num <= 265) || (num >= 560 && num <= 563) || num == 270 || num == 330 || num == 572 || num == 573 || num == 662 || (num >= 667 && num <= 672)){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("WOMST")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("DEN")){
                int num = here.number;
                if(num == 210){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            if(here.prefix.equals("FSHS")){
                electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
            }
            if(here.prefix.equals("GNHE")){
                int num = here.number;
                if(num == 310){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 1));
                }
            }
            
            //CIS - Tech Electives
            if(here.prefix.equals("CIS")){
                int num = here.number;
                if((num >= 500 && num <= 699)){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 2));
                }
            }
            
            //CIS - Natural Sciences Electives
            if(here.prefix.equals("BIO")){
                int num = here.number;
                if(num == 198 || num == 201){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 3));
                }
            }
            if(here.prefix.equals("CHM")){
                int num = here.number;
                if(num == 210 || num == 230){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 3));
                }
            }
            if(here.prefix.equals("PHYS")){
                int num = here.number;
                if(num == 213 || num == 214 || num == 223 || num == 224){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 3));
                }
            }
            
            
            //CIS - Communication Electives
            if(here.prefix.equals("COMM")){
                int num = here.number;
                if(num == 322 || num == 326){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 4));
                }
            }
            if(here.prefix.equals("MANGT")){
                int num = here.number;
                if(num == 420){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 4));
                }
            }
            if(here.prefix.equals("THTRE")){
                int num = here.number;
                if(num == 261 || num == 265){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 4));
                }
            }
            
            //CIS Masters Electives
            if(here.prefix.equals("CIS")){
                int num = here.number;
                if(num == 706 || num == 722 || num == 736 || num == 690){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 6));
                }
                if(num == 705 || num == 706 || num == 771 || num == 806){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 7));
                }
                if(num == 720 || num == 721 || num == 722 || num == 725 || num == 726){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 8));
                }
                if(num == 730 || num == 740 || num == 761){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 9));
                }
                if(num == 770 || num == 775){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 10));
                }
                if((num >= 800 && num <= 900) && num != 895 && num != 897 && num != 898 && num != 899  && num != 999){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 11));
                }
                if(num == 734 || num == 834 || num == 732 || num == 833 || num == 725 || num == 844 || num == 730 || num == 732 || num == 830 || num == 751 || num == 755 || num == 726){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 12));
                }
                if(num >= 700 && num <= 999){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 13));
                }
                if(num >= 600 && num <= 999){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 14));
                }
                if(num >= 800 && num <= 999){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 15));
                }
                if(num == 999){
                    electiveListCourses.add(new ElectiveListCourse(electiveListCourses.size(), here.id, 16));
                }
            }
        }
    }
    
    public static void scrapeUG(){
        String baseURI = "http://catalog.k-state.edu/content.php?catoid=13&navoid=1425&print&expand=1&filter[cpage]=";
        Document doc = null;
        int page = 1;
        boolean cont = true;
        while(cont){
            try{
                doc = Jsoup.connect(baseURI + page).timeout(10000).get();
            }catch(Exception e){
                e.printStackTrace();
                break;
            }
            Element content = doc.getElementsByClass("block_content").first();
            Elements tables = content.getElementsByTag("table");
            Element table = tables.get(2);
            Elements course_table = table.children();
            Element tbody = course_table.first();
            Elements table_rows = tbody.children();
            Elements courses = table_rows.select("li");
            String original_desc;
            for(Element course_html : courses){
                String fulltitle = course_html.getElementsByTag("h3").first().html();
                if(fulltitle.length() <= 1){
                    System.out.println("I must be at the end of the course listing. Page " + page + " is blank!");
                    cont = false;
                    break;
                }
                Course course = new Course();
                course.prefix = fulltitle.substring(0, fulltitle.indexOf(" "));
                fulltitle = fulltitle.substring(fulltitle.indexOf(" ")+1, fulltitle.length());
                course.setNumber(fulltitle.substring(0, fulltitle.indexOf(" ")));
                course.title = fulltitle.substring(fulltitle.indexOf(" ")+3, fulltitle.length());
                String description = course_html.html();
                //System.out.println(description);
                original_desc = description;
                description = description.substring(description.indexOf("</h3>") + 6, description.length());
                String credits = description.substring(0, description.indexOf("\n"));
                course.setHours(credits.substring(credits.indexOf('(') + 1, credits.indexOf(')')));
                description = description.substring(description.indexOf("\n")+1, description.length());
                while(description.length() > 0){
                    if(description.startsWith("<strong>")){
                        String item = description.substring(8, description.indexOf("</strong>")).trim();
                        description = description.substring(description.indexOf("</strong>")+9).trim();
                        String value = "";
                        if(description.contains("<strong>")){
                            value = description.substring(0, description.indexOf("<strong>"));
                            description = description.substring(description.indexOf("<strong>"), description.length());
                        }else{
                            value = description;
                            description = "";
                        }
                        value = value.replace("<br /> ", "\n").trim();
                        value = value.replace("<br />", "\n").trim();
                        if(item.contains("Requisites")){
                            course.requisites = value;
                        }else if(item.contains("UGE course")){
                            if(value.contains("Yes")){
                                course.uge = true;
                            }else{
                                course.uge = false;
                            }
                        }else if(item.contains("K-State 8")){
                            course.kstate8 = value;
                        }else if(item.contains("Note")){
                            course.notes = value;
                        }else if(item.contains("When Offered")){
                            course.semesters = value;
                        }else if(item.contains("Cross-listed")){
                            course.crosslisted = value;
                        }else{
                            System.out.print("ERROR: CANNOT PARSE ITEM! ");
                            System.out.println(item + " - " + value);
                        }
                    }else if(description.startsWith("<hr />")){
                        description = description.substring(6).trim();
                        if(!description.startsWith("<")){
                            course.description = description.substring(0, description.indexOf("<br />")).trim();
                            description = description.substring(description.indexOf("<br />") + 6, description.length()).trim();
                        }
                    }else if(description.startsWith("<br />")){
                        description = description.substring(6).trim();
                    }else{
                        System.out.print("ERROR: CANNOT PARSE TEXT ");
                        System.out.println(description);
                    }
                }
                if(course.description == null || course.description.length() == 0){
                    course.description = course.title;
                }
//                if(course.hours == null || course.hours.length() == 0){
//                    course.hours = "Variable";
//                }
                course.ugrad = true;
                course.id = count++;
                if(!course.verify()){
                    System.out.println("Problems!");
                }
                courselist.add(course);
            }
            System.out.println("Page " + page + " complete! " + count + " courses processed so far...");
            page++;
        }
    }
    
    public static void scrapeGrad(){
        String baseURI = "http://catalog.k-state.edu/content.php?catoid=2&navoid=11&print&expand=1&filter[cpage]=";
        Document doc = null;
        int page = 1;
        boolean cont = true;
        while(cont){
            try{
                doc = Jsoup.connect(baseURI + page).timeout(10000).get();
            }catch(Exception e){
                e.printStackTrace();
                break;
            }
            Element content = doc.getElementsByClass("block_content").first();
            Elements tables = content.getElementsByTag("table");
            Element table = tables.get(2);
            Elements course_table = table.children();
            Element tbody = course_table.first();
            Elements table_rows = tbody.children();
            Elements courses = table_rows.select("li");
            String original_desc;
            for(Element course_html : courses){
                String fulltitle = course_html.getElementsByTag("h3").first().html();
                if(fulltitle.length() <= 1){
                    System.out.println("I must be at the end of the course listing. Page " + page + " is blank!");
                    cont = false;
                    break;
                }
                Course course = new Course();
                course.prefix = fulltitle.substring(0, fulltitle.indexOf(" "));
                fulltitle = fulltitle.substring(fulltitle.indexOf(" ")+1, fulltitle.length());
                course.setNumber(fulltitle.substring(0, fulltitle.indexOf(" ")));
                course.title = fulltitle.substring(fulltitle.indexOf(" ")+3, fulltitle.length());
                String description = course_html.html();
                //System.out.println(description);
                original_desc = description;
                description = description.substring(description.indexOf("</h3>") + 6, description.length());
                description = description.substring(description.indexOf("\n")+1, description.length());
                while(description.length() > 0){
                    if(description.startsWith("<strong>")){
                        String item = description.substring(8, description.indexOf("</strong>")).trim();
                        description = description.substring(description.indexOf("</strong>")+9).trim();
                        String value = "";
                        if(description.contains("<strong>")){
                            value = description.substring(0, description.indexOf("<strong>"));
                            description = description.substring(description.indexOf("<strong>"), description.length());
                        }else{
                            value = description;
                            description = "";
                        }
                        value = value.replace("<br /> ", "\n").trim();
                        value = value.replace("<br />", "\n").trim();
                        if(item.contains("Requisites")){
                            course.requisites = value;
                        }else if(item.contains("UGE course")){
                            if(value.contains("Yes")){
                                course.uge = true;
                            }else{
                                course.uge = false;
                            }
                        }else if(item.contains("K-State 8")){
                            course.kstate8 = value;
                        }else if(item.contains("Note")){
                            course.notes = value;
                        }else if(item.contains("When Offered")){
                            course.semesters = value;
                        }else if(item.contains("Cross-listed") || item.contains("Crosslisted")){
                            course.crosslisted = value;
                        }else if (item.contains("Credits")){
                            course.setHours(value.replace("(", "").replace(")", ""));
                        }else{
                            System.out.print("ERROR: CANNOT PARSE ITEM! ");
                            System.out.println(item + " - " + value);
                        }
                    }else if(description.startsWith("<hr />")){
                        description = description.substring(6).trim();
                        if(description.length() == 0){
                            continue;
                        }
                        if(!description.startsWith("<")){
                            course.description = description.substring(0, description.indexOf("<br /><br />")).trim();
                            description = description.substring(description.indexOf("<br /><br />") + 12, description.length()).trim();
                        }else if(description.startsWith("<span") || description.startsWith("<p") || description.startsWith("<div")){
                            description = description.substring(description.indexOf(">") + 1);
                            course.description = description.substring(0, description.indexOf("<br /><br />")).trim();
                            course.description = course.description.replace("<span>", "");
                            course.description = course.description.replace("</span>", "");
                            course.description = course.description.replace("&nbsp;", "");
                            course.description = course.description.replace("</p>", "");
                            course.description = course.description.replace("<br />", "");
                            course.description = course.description.replace("</div>", "");
                            course.description = course.description.trim();
                            description = description.substring(description.indexOf("<br /><br />") + 12, description.length()).trim();
                        }
                    }else if(description.startsWith("<br />")){
                        description = description.substring(6).trim();
                    }else{
                        if(description.startsWith("<")){
                            description = description.substring(description.indexOf(">") + 1);
                        }else if(description.startsWith("&nbsp;")){
                            description = description.substring(description.indexOf(";") + 1);
                        }else {
                            System.out.print("ERROR: CANNOT PARSE TEXT ");
                            System.out.println(description);
                        }
                    }
                }
                if(course.description == null || course.description.length() == 0){
                    course.description = course.title;
                }
//                if(course.hours == null || course.hours.length() == 0){
//                    course.hours = "Variable";
//                }
                if(courselist.contains(course)){
                    Course temp = courselist.get(courselist.indexOf(course));
                    temp.grad = true;
                }else{
                    course.id = count++;
                    course.grad = true;
                    if(!course.verify()){
                        System.out.println("Problems!");
                    }
                    courselist.add(course);
                }
            }
            System.out.println("Page " + page + " complete! " + count + " courses processed so far...");
            page++;
        }
    }
}
