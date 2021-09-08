CREATE DATABASE AnalysisCountSteps/*СОЗДАНИЕ БАЗЫ ДАННЫХ*/
GO 

USE AnalysisCountSteps
CREATE TABLE Users /*СОЗДАНИЕ ТАБЛИЦЫ*/
(
	Rank_ INT,
	User_ NVARCHAR(50),
	Status_ VARCHAR(50),
	Steps INT
);
GO 

CREATE TABLE Statistics_  /*СОЗДАНИЕ ТАБЛИЦЫ СТАТИСТИКА*/
(
	ФИО NVARCHAR(50),
	СреднееКолВоШагов FLOAT,
	ЛучшийРезультат INT,
	ХудшийРезультат INT
);
GO 

/*ОБЪЯВЛЕНИЕ,ИНИЦИАЛИЗАЦИЯ ДАННЫХ В ПЕРЕМЕННЫЕ С ПОСЛЕДУЮЩЕЙ ВСТАВКОЙ В ТАБЛИЦУ*/
DECLARE @Name NVARCHAR(50), @Average FLOAT, @TheBest INT, @TheWorst INT
SET @Name='Нарастающий Гавр'

/*ЗАПИСЬ ПОЛУЧЕННЫХ ЗАПРОСОВ В ПЕРЕМЕННЫЕ*/
SET @Average=(SELECT AVG(Steps) FROM Users WHERE User_='Нарастающий Гавр')
SET @TheBest=(SELECT MAX(Steps) FROM Users WHERE User_='Нарастающий Гавр')
SET @TheWorst=(SELECT MIN(Steps) FROM Users WHERE User_='Разнуликов Павел')

/*ЗАПИСЬ ЗНАЧЕНИЙ В ТАБЛИЦУ*/
INSERT INTO Statistics_ (ФИО,СреднееКолВоШагов,ЛучшийРезультат,ХудшийРезультат) VALUES(@Name,@Average,@TheBest,@TheWorst)

/*ЗАПРОС НА КОЛ-ВО ЗАПИСЕЙ В НОВОЙ ТАБЛИЦЕ*/
SELECT COUNT(*) FROM Statistics_

/*ДОБАВЛЕНИЕ ДВУХ СТОЛБЦЕВ Статус и Ранг*/
ALTER TABLE Statistics_ 
ADD [Статус] VARCHAR(15) NOT NULL DEFAULT 'Finished'

ALTER TABLE Statistics_ 
ADD [Ранг] INT  

/*ОБЪЯВЛЕНИЕ,ИНИЦИАЛИЗАЦИЯ ДАННЫХ В ПЕРЕМЕННУЮ С ПОСЛЕДУЮЩИМ ДОБАВАЛЕНИЕМ*/
DECLARE @Rank INT

/*ЗАПИСЬ ПОЛУЧЕННЫХ ЗАПРОСОВ В ПЕРЕМЕННЫЕ*/
SET @Rank=(SELECT AVG(Rank_) AS 'Ранг' FROM Users WHERE User_='Нарастающий Гавр')

/*ЗАПИСЬ ЗНАЧЕНИЙ В ТАБЛИЦУ*/
UPDATE Statistics_ SET Ранг=@Rank WHERE ФИО='Нарастающий Гавр'