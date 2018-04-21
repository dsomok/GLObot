﻿using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Serialization.Types;
using Xunit;

namespace Telegram.Bot.GLObot.UnitTests.CheckinStatsCalculatorTests
{
    public class RealLifeScenarios
    {
        [Fact]
        public void RealLifeScenario1()
        {
            var checkinEvents = CheckinEventsListPrimer1();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMilliseconds.ShouldBe(17329000); //4:48:49 Igor 2018\\/02\\/28
        }

        [Fact]
        public void RealLifeScenario2()
        {
            var checkinEvents = CheckinEventsListPrimer2();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMilliseconds.ShouldBe(16661000); //4:37:31 Sasha   2018\\/02\\/28
        }

        [Fact]
        public void RealLifeScenario3()
        {
            var checkinEvents = CheckinEventsListPrimer3();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMilliseconds.ShouldBe(17126000); //4:45:26 Den  2018\\/02\\/28
        }

        [Fact]
        public void RealLifeScenario4()
        {
            var checkinEvents = CheckinEventsListPrimer4();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMilliseconds.ShouldBe(27979000); //07:46:19 Dima  2018\\/03\\/01
        }

        private List<CheckinEvent> CheckinEventsListPrimer1()
        {
            string teleportsCase =
                "[{\"timestamp\":\"2018\\/02\\/28 10:54:02\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:01:11\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:01:52\",\"locationid\":86,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:02:00\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:26:55\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:27:01\",\"locationid\":86,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:27:39\",\"locationid\":87,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:27:45\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:52:32\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:53:13\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 11:56:28\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 12:03:57\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:07\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:31\",\"locationid\":116,\"direction\":\"in\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:57:54\",\"locationid\":116,\"direction\":\"out\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:58:13\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:59:47\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 13:00:34\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 13:00:34\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 13:03:19\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 13:55:14\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 14:00:36\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:11\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:42\",\"locationid\":55,\"direction\":\"in\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:02:38\",\"locationid\":55,\"direction\":\"out\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:03:15\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 15:06:56\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 15:27:47\",\"locationid\":55,\"direction\":\"in\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:40:28\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:01:36\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:01:44\",\"locationid\":87,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:45\",\"locationid\":87,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:55\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:48:15\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:48:19\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:56:11\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:10:28\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:12:29\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 18:18:37\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:48:48\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true}]";

            return CheckinEvent.FromJson(teleportsCase);
        }

        private List<CheckinEvent> CheckinEventsListPrimer2()
        {
            string teleportsCase =
                "[{\"timestamp\":\"2018\\/02\\/28 11:31:19\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 11:52:36\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:03:55\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:10\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:37\",\"locationid\":116,\"direction\":\"in\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:57:52\",\"locationid\":116,\"direction\":\"out\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:58:18\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 13:00:41\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 13:10:41\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 14:00:41\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:14\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:44\",\"locationid\":55,\"direction\":\"in\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:03:05\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 15:06:52\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 15:06:57\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:03:07\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:03:14\",\"locationid\":87,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:39\",\"locationid\":87,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:47\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:56:13\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 17:12:42\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 17:12:51\",\"locationid\":87,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 17:25:38\",\"locationid\":87,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 17:25:44\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:10:32\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:11:19\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 18:18:46\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:37:08\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 18:55:34\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 19:10:07\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true}]";

            return CheckinEvent.FromJson(teleportsCase);
        }

        private List<CheckinEvent> CheckinEventsListPrimer3()
        {
            string teleportsCase =
                "[{\"timestamp\":\"2018\\/02\\/28 11:54:14\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 12:03:53\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:05\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:48:35\",\"locationid\":116,\"direction\":\"in\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:57:50\",\"locationid\":116,\"direction\":\"out\",\"area\":\"Office KBP2-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:58:16\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 12:59:51\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:00:46\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:36\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 14:01:58\",\"locationid\":55,\"direction\":\"in\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:02:33\",\"locationid\":55,\"direction\":\"out\",\"area\":\"Location-KBP5C-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/02\\/28 15:03:02\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:01:53\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:02:03\",\"locationid\":87,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:02:04\",\"locationid\":87,\"direction\":\"in\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:48\",\"locationid\":87,\"direction\":\"out\",\"area\":\"Office KBP3-C\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:38:54\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 16:56:09\",\"locationid\":108,\"direction\":\"in\",\"area\":\"Office KBP3-R\",\"working\":true},\r\n{\"timestamp\":\"2018\\/02\\/28 19:10:08\",\"locationid\":108,\"direction\":\"out\",\"area\":\"Office KBP3-R\",\"working\":true}]";

            return CheckinEvent.FromJson(teleportsCase);
        }

        private List<CheckinEvent> CheckinEventsListPrimer4()
        {
            string teleportsCase =
                "[{\"timestamp\":\"2018\\/03\\/01 11:48:26\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 11:54:17\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 11:54:26\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 12:27:42\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 12:27:48\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 12:28:59\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 12:38:29\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 12:43:36\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 12:43:44\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:36:45\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:36:50\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 13:36:50\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 13:38:10\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:39:35\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:44:33\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 13:44:41\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:45:03\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 13:47:10\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 14:26:32\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 14:26:49\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 15:01:24\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 15:01:31\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:21:36\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:21:43\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 16:21:44\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 16:22:59\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:24:33\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:28:30\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 16:28:38\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:28:57\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 16:42:40\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 18:18:37\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 18:18:44\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 18:19:57\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 18:21:06\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 18:25:50\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 18:25:57\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 20:01:14\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 20:03:00\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 20:04:14\",\"locationid\":165,\"direction\":\"in\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 20:04:53\",\"locationid\":165,\"direction\":\"out\",\"area\":\"G-club\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 20:10:31\",\"locationid\":95,\"direction\":\"in\",\"area\":\"Office KBP3-L\",\"working\":true},\r\n{\"timestamp\":\"2018\\/03\\/01 20:10:38\",\"locationid\":196,\"direction\":\"in\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 21:00:00\",\"locationid\":196,\"direction\":\"out\",\"area\":\"Location-KBP3L-Rubikon-R\",\"working\":false},\r\n{\"timestamp\":\"2018\\/03\\/01 21:00:06\",\"locationid\":95,\"direction\":\"out\",\"area\":\"Office KBP3-L\",\"working\":true}]";

            return CheckinEvent.FromJson(teleportsCase);
        }
    }
}
