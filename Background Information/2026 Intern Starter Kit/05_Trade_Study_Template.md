# Trade Study Template

Use this template to conduct trade study analyses for component selection decisions. Each team should complete at least one trade study for their project (e.g., game engine selection, sensor alternatives, enclosure material, microcontroller selection).

## Trade Study: [Title]

**Date:** [Date]
**Author(s):** [Team Member Names]
**Decision Required:** [Brief description of what needs to be decided]

---

## 1. Purpose

Describe why this trade study is being conducted and what decision it will inform.

## 2. Candidates

List the options being evaluated:

| ID | Candidate | Description |
|----|-----------|-------------|
| A  | [Name]    | [Brief description] |
| B  | [Name]    | [Brief description] |
| C  | [Name]    | [Brief description] |

## 3. Evaluation Criteria

Define the criteria and their relative weights. Weights should sum to 100%.

| Criterion | Weight | Description |
|-----------|--------|-------------|
| Cost | __% | Purchase price and any recurring costs |
| Performance | __% | How well it meets functional requirements |
| Ease of Integration | __% | Compatibility with existing systems |
| Reliability | __% | Expected lifespan, failure rate (MTBF) |
| Size / Weight | __% | Physical footprint and mass |
| Power Consumption | __% | Electrical power requirements |
| Availability | __% | Lead time, stock availability |
| Documentation / Support | __% | Quality of docs, community, vendor support |
| **Total** | **100%** | |

## 4. Scoring Matrix

Score each candidate on each criterion (1-5 scale):
- 1 = Poor / Does not meet requirements
- 2 = Below average / Partially meets requirements
- 3 = Average / Meets requirements
- 4 = Good / Exceeds requirements
- 5 = Excellent / Significantly exceeds requirements

| Criterion | Weight | Candidate A ||| Candidate B ||| Candidate C |||
|-----------|--------|------|---------|----------|------|---------|----------|------|---------|----------|
|           |        | Score | Notes | Weighted | Score | Notes | Weighted | Score | Notes | Weighted |
| Cost      | __%    |       |        |          |       |        |          |       |        |          |
| Performance | __%  |       |        |          |       |        |          |       |        |          |
| Ease of Integration | __% | | |    |       |        |          |       |        |          |
| Reliability | __%  |       |        |          |       |        |          |       |        |          |
| Size / Weight | __% |      |        |          |       |        |          |       |        |          |
| Power | __%        |       |        |          |       |        |          |       |        |          |
| Availability | __% |       |        |          |       |        |          |       |        |          |
| Docs / Support | __% |     |        |          |       |        |          |       |        |          |
| **Total** | **100%** |     |        | **___**  |       |        | **___**  |       |        | **___**  |

**Weighted Score** = Score × (Weight / 100)

## 5. Analysis

Discuss the results:
- Which candidate scored highest overall?
- Were there any criteria where scores were very close?
- Are there any disqualifying factors not captured by the scoring (e.g., export restrictions, TAA compliance)?
- Sensitivity analysis: if you changed the weights slightly, would the winner change?

## 6. Recommendation

State the recommended candidate and provide justification:

**Recommended:** [Candidate X]

**Rationale:** [2-3 sentences explaining why, including key advantages and acceptable trade-offs]

## 7. Risks and Mitigations

| Risk | Impact | Likelihood | Mitigation |
|------|--------|------------|------------|
| [Risk 1] | High/Med/Low | High/Med/Low | [How to mitigate] |
| [Risk 2] | High/Med/Low | High/Med/Low | [How to mitigate] |

---

## Example: Microcontroller Trade Study

Below is a partially-filled example for reference:

**Decision Required:** Select the microcontroller for the trigger button hardware.

| Criterion | Weight | Arduino Nano | Score | Arduino Uno | Score | ESP32 | Score |
|-----------|--------|-------------|-------|-------------|-------|-------|-------|
| Cost | 20% | ~$5 (clone) | 5 | ~$10 | 4 | ~$8 | 4 |
| Ease of Integration | 30% | Simple, USB serial | 5 | Simple, USB serial | 5 | WiFi focus, more complex | 3 |
| Size | 15% | Very small | 5 | Medium | 3 | Small | 4 |
| Reliability | 15% | Proven | 4 | Proven | 5 | Proven | 4 |
| Documentation | 20% | Excellent | 5 | Excellent | 5 | Good | 4 |

**Recommendation:** Arduino Nano — smallest form factor, lowest cost, identical programming interface to Uno, fits easily in a handheld enclosure.
