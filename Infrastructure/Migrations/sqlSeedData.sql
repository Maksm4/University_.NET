USE EfCore;

INSERT INTO Student
VALUES 
	('john', 'doe', 'john.doe@gmail.com', '2004/11/17'),
	('mary', 'skibidi', 'mary.skibidi@gmail.com', '1994/1/17');


INSERT INTO Course
VALUES 
	('Mathematics', 'Introduction', 0),
	('compter science', 'intro', 0);

INSERT INTO LearningPlan
VALUES
	('backelors in cs', 1),
	('masters in cs', 2);


INSERT INTO CourseModule
VALUES ( 1, 'algebra and calc'),
		(2, 'programming fundamentals');

INSERT INTO EnrolledCourse
VALUES 
	(1, 1, '2023/11/19', '2024/11/12'),
	(2,2, '2024/10/17', '2024/10/19');

INSERT INTO ModuleMark
VALUES 
	(1,1,1,4),
	(2,2,2,5);