from pathlib import Path


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = f.readlines()

    def handle_line(self, line):
        line_len = len(line)

        max_10 = 0
        max_1 = 0

        for i, c in enumerate(line):
            try:
                num = int(c)
                if i < line_len - 2 and num > max_10:
                    max_10 = num
                    max_1 = 0
                elif num > max_1:
                    max_1 = num
            except Exception:
                continue

        res = max_10 * 10 + max_1
        return res

    def get_day_solution(self):
        return str(sum(map(self.handle_line, self.input_data)))
